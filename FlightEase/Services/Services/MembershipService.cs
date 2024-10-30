using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Repositories.Repositories;

namespace FlightEaseDB.BusinessLogic.Services
{

	public interface IMembershipService
	{
		public MemBershipDTO CreateMembership(MemBershipDTO membershipCreate);
		public MemBershipDTO UpdateMembership(MemBershipDTO membershipUpdate);
		public bool DeleteMembership(int idTmp);
		public List<MemBershipDTO> GetAll();
		public MemBershipDTO GetById(int idTmp);
	}

	public class MembershipService : IMembershipService
	{
		private readonly IMembershipRepository _membershipRepository;
        private readonly IOrderDetailService _orderDetailService;

        public MembershipService(IMembershipRepository membershipRepository, IOrderDetailService orderDetailService)
		{
			_membershipRepository = membershipRepository;
            _orderDetailService = orderDetailService;
        }

		public MemBershipDTO CreateMembership(MemBershipDTO membershipCreate)
		{
			var membership = new Membership
			{
				Rank = membershipCreate.Rank,
				Discount = membershipCreate.Discount
			};

			_membershipRepository.Create(membership);
			_membershipRepository.Save();

			membershipCreate.MembershipId = membership.MembershipId; // Lấy ID từ entity sau khi lưu
			return membershipCreate;
		}

		public MemBershipDTO UpdateMembership(MemBershipDTO membershipUpdate)
		{
			var membershipEntity = new Membership
			{
				MembershipId = membershipUpdate.MembershipId,
				Rank = membershipUpdate.Rank,
				Discount = membershipUpdate.Discount
			};
			var result = _membershipRepository.Update(membershipEntity);
			return new MemBershipDTO
			{
				MembershipId = result.MembershipId,
				Rank = result.Rank,
				Discount = result.Discount
			};
		}


		public bool DeleteMembership(int idTmp)
		{
			var existingMembership = _membershipRepository.Get(idTmp);
			if (existingMembership == null) return false;

			// Gọi phương thức xóa từ BaseRepository
			_membershipRepository.Delete(existingMembership);
			_membershipRepository.Save();

			return true;
		}

		public List<MemBershipDTO> GetAll()
		{
			var membershipEntities = _membershipRepository.Get();
			return membershipEntities.Select(m => new MemBershipDTO
			{
				MembershipId = m.MembershipId,
				Rank = m.Rank,
				Discount = m.Discount
			}).ToList();
		}

		public MemBershipDTO GetById(int idTmp)
		{
			var membershipEntity = _membershipRepository.Get(idTmp);
			if (membershipEntity == null) return null;
			return new MemBershipDTO
			{
				MembershipId = membershipEntity.MembershipId,
				Rank = membershipEntity.Rank,
				Discount = membershipEntity.Discount
			};
		}
        public async Task UpdateMembershipStatus(int userId)
        {
            // Tính tổng chi tiêu
            var totalSpent = await _orderDetailService .GetTotalSpendingAsync(userId);

            var membership =  _membershipRepository.Get(userId);
            if (membership == null) return;

            // Cập nhật trạng thái membership dựa trên tổng tiền đã chi tiêu
            if (totalSpent >= 1000) // Ví dụ: Tổng tiền >= 1000
            {
                membership.Rank = "Gold";
                membership.Discount = 15; // Giảm giá 15%
            }
            else if (totalSpent >= 500) // Tổng tiền >= 500
            {
                membership.Rank = "Silver";
                membership.Discount = 10; // Giảm giá 10%
            }
            else
            {
                membership.Rank = "Bronze";
                membership.Discount = 5; // Giảm giá 5%
            }

            _membershipRepository.Update(membership);
            await _membershipRepository.SaveAsync(); // Lưu thay đổi
        }
    }


}
