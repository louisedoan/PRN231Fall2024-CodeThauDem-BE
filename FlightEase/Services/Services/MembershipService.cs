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


        /*Task UpdateMembershipStatus(int userId);*/
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
        /*public async Task UpdateMembershipStatus(int userId)
        {
            // Lấy tổng chi tiêu của người dùng
            var totalSpent = await _orderDetailService.GetTotalSpendingAsync(userId);
            if (totalSpent == null) return;

            // Tìm kiếm `Membership` của người dùng
            var membership = _membershipRepository.Get(userId);
            if (membership == null) return;

            // Cập nhật `Rank` và `Discount` dựa trên tổng tiền đã chi tiêu
            if (totalSpent >= 8000000)
            {
                membership.Rank = "Gold";
                membership.Discount = 5; // Giảm giá 5%
            }
            else if (totalSpent >= 5000000)
            {
                membership.Rank = "Silver";
                membership.Discount = 3; // Giảm giá 3%
            }
            else
            {
                membership.Rank = "Bronze";
                membership.Discount = 0; // Không có giảm giá
            }

            _membershipRepository.Update(membership);
            await _membershipRepository.SaveAsync();
        }*/
    }
}
