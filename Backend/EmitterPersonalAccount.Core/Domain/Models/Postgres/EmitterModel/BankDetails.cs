using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel
{
    public class BankDetails : ValueObject
    {
        private BankDetails()
        {
            
        }
        private BankDetails(
            string bik,
            string bankName,
            string settlementAccount,
            string correspondentAccount,
            string bankINN,
            string department,
            string customerAccount,
            string taxBenefits,
            string country
            )
        {
            BIK = bik;
            BankName = bankName;
            SettlementAccount = settlementAccount;
            CorrespondentAccount = correspondentAccount;
            BankINN = bankINN;
            Department = department;
            CustomerAccount = customerAccount;
            TaxBenefits = taxBenefits;
            Country = country;
        }
        public string BIK { get; private set; }
        public string BankName { get; private set; }
        public string SettlementAccount { get; private set; } = null!;
        public string CorrespondentAccount { get; private set; }
        public string BankINN { get; private set; } = null!;
        public string Department { get; private set; } = null!;
        public string CustomerAccount { get; private set; }
        public string TaxBenefits { get; private set; }
        public string Country { get; private set; } = null!;
        public static Result<BankDetails> Create(
            string bik,
            string bankName,
            string settlementAccount,
            string correspondentAccount,
            string bankINN,
            string department,
            string customerAccount,
            string taxBenefits,
            string country
            )
        {
            return Result.Success(new BankDetails(
                bik, 
                bankName, 
                settlementAccount, 
                correspondentAccount, 
                bankINN, 
                department, 
                customerAccount, 
                taxBenefits, 
                country));
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return BIK;
            yield return BankName;
            yield return SettlementAccount;
            yield return CorrespondentAccount;
            yield return BankINN;
            yield return Department;
            yield return CustomerAccount;
            yield return TaxBenefits;
            yield return Country;
        }
    }
}
