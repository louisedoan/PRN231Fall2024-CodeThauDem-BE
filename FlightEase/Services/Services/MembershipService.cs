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

		public MembershipService(IMembershipRepository membershipRepository)
		{
			_membershipRepository = membershipRepository;
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
	}


}
