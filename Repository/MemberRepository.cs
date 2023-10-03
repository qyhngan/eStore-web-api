using DataAccess.DTO.Member;
using DataAccess.Mapper;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repository
{
    public class MemberRepository : IMemberRepository
    {
        public MemberResponse Login(string email, string password) => Mapper.MapMemberToDTO(MemberDAO.FindMemberByEmailAndPassword(email, password), CheckRole(email));
        public string CheckRole(string email) => MemberDAO.CheckRole(email);
        public IEnumerable<MemberResponse> GetAllMember()
        {
            List<MemberResponse> memberResponses = new List<MemberResponse>(); 
            var members = MemberDAO.GetMembers();
            if (members == null)
            {
                return null;
            }
            foreach (var member in members)
            {
                memberResponses.Add(Mapper.MapMemberToDTO(member));
            }
            return memberResponses;
        }
        public MemberResponse GetMemberById(int id) => Mapper.MapMemberToDTO(MemberDAO.GetMemberById(id));

        public int AddMember(MemberRequest member) => MemberDAO.AddMember(member);

        public void DeleteMember(int id) => MemberDAO.DeleteMember(id);

        public void UpdateMember(MemberResponse member) => MemberDAO.UpdateMember(member);

        public MemberResponse LoginAdmin(string email, string password) => Mapper.MapMemberToDTO(MemberDAO.LoginAdmin(email, password), CheckRole(email));
    }
}
