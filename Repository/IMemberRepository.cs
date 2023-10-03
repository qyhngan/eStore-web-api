using BusinessObject;
using DataAccess.DTO.Member;
using System;
using System.Collections.Generic;

namespace Repository
{
    public interface IMemberRepository
    {
        MemberResponse Login(string email, string password);
        string CheckRole(string email);
        IEnumerable<MemberResponse> GetAllMember();
        MemberResponse GetMemberById(int id);
        int AddMember(MemberRequest member);
        void DeleteMember(int id);
        void UpdateMember(MemberResponse member);
        MemberResponse LoginAdmin(string email, string password);
    }
}
