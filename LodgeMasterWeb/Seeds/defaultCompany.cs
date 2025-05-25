namespace LodgeMasterWeb.Seeds
{
    public static class defaultCompany
    {

        public static async Task<string> SeedCompanyAndBrancheAsync(ApplicationDbContext _context)
        {
            try
            {

                var companyData = _context.Companies.AsNoTracking().Any();
                if (companyData == true)
                {

                    //return "";
                    return _context.Companies.AsNoTracking().FirstOrDefault().CompanyID;
                }
                var newCompanyID = Guid.NewGuid().ToString();
                var newBrancheID = Guid.NewGuid().ToString();

                var defaultCompany = new Company
                {

                    CompanyID = newCompanyID,
                    CompanyName_E = "DemoCompany",
                    CompanyName_A = "DemoCompany",
                    Companylogin = "DemoCompany",
                    MasterEmail = string.Empty,
                    Address = string.Empty,
                    Phone = string.Empty,
                    PersonName = string.Empty,
                    Mobile = string.Empty,
                    Email = string.Empty,
                    CompanyFolder = "DemoCompany",
                    bActive = 1,
                    StartDate = 0,
                    EndDate = 0,
                    CounterUsers = 0,
                    isDemo = 0,
                    isDeleted = 0
                };

                _context.Companies.Add(defaultCompany);

                var defaultCompanyBranche = new CompanyBranche
                {
                    BrancheID = newBrancheID,
                    CompanyID = newCompanyID,
                    BrancheName = "DemoBranche",
                    bActive = 1,
                    BrancheDesc = string.Empty

                };
                _context.CompanyBarnches.Add(defaultCompanyBranche);

                //var LinkCompanyBranche = new CompanyLinkBrancheUser
                //{
                //    LinkCBID = Guid.NewGuid().ToString(),
                //    CompanyID = newCompanyID,
                //    BrancheID = newBrancheID
                //};
                //_context.CompanyLinkBranches.Add(LinkCompanyBranche);

                //_context.SaveChanges();

                return newCompanyID;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
    }
}
