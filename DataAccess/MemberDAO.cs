using BusinessObject;
using DataAccess.DTO.Member;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MemberDAO
    {
        public static Member FindMemberByEmailAndPassword(string email, string password) 
        {
            Member member = new Member();
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    member  = dbContext.Members.SingleOrDefault(x => x.Email == email && x.Password == password);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public static string CheckRole(string email)
        {
            string role = "member";
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
                string adminEmail = config["Admin:Email"];
                if (email == adminEmail) role = "admin";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return role;
        }

        public static Member LoginAdmin(string email, string password)
        {
            string role = "member";
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
                string pw = config["Admin:Password"];
                if (CheckRole(email) == "admin" && password == pw) return new Member { Email = email, Password = password};
                else return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<Member> GetMembers()
        {
            List<Member> members = new List<Member>();
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    members = dbContext.Members.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return members;
        }

        public static Member GetMemberById(int id)
        {
            Member member = new Member();
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    member = dbContext.Members.SingleOrDefault(x => x.MemberId == id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public static int AddMember(MemberRequest member)
        {
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    int id = 0;

                    if (dbContext.Members.Count() > 0) id = dbContext.Members.Max(x => x.MemberId);

                    Member m = new Member() { 
                        City = member.City,
                        CompanyName = member.CompanyName,
                        Country = member.Country,
                        Email = member.Email,
                        Password = member.Password,
                        MemberId = id + 1
                    };

                    dbContext.Members.Add(m);
                    dbContext.SaveChanges();
                    return m.MemberId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteMember(int id)
        {
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    var member = dbContext.Members.SingleOrDefault(x => x.MemberId == id);
                    if (member == null) throw new Exception("Member doesn't exist");
                    dbContext.Members.Remove(member);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public static void UpdateMember(MemberResponse member)
        {
            try
            {
                using (var dbContext = new FStoreDBContext())
                {
                    Member m = GetMemberById(member.MemberId);
                    if (m == null) throw new Exception("Member not found");
                    m.Email = member.Email;
                    m.CompanyName = member.CompanyName;
                    m.City = member.City;
                    m.Country = member.Country;
                    dbContext.Members.Update(m);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
