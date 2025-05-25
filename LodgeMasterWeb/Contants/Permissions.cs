namespace LodgeMasterWeb.Contants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsList(string module)
        {

            var permissions = new List<string>()
            {
                $"Permissions.{module}.View",
                $"Permissions.{module}.Create",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };

            if (module == "StaffManagement")
            {
                permissions.Add($"Permissions.{module}.AllDepartments");
            }
            if (module == "Report")
            {
                permissions.Clear();
                permissions.Add($"Permissions.{module}.Analysis");
                permissions.Add($"Permissions.{module}.Orders");
                permissions.Add($"Permissions.{module}.Items");
                permissions.Add($"Permissions.{module}.Inspection");

            }
            return permissions;
        }
        public static List<string> GenerateAllPermissions()
        {
            var allPermissions = new List<string>();

            var modules = Enum.GetValues(typeof(moduleScreen));

            foreach (var module in modules)
                allPermissions.AddRange(GeneratePermissionsList(module.ToString()));

            return allPermissions;
        }

        public static class CreateOrders
        {
            public const string View = "Permissions.CreateOrders.View";
            public const string Create = "Permissions.CreateOrders.Create";
            public const string Edit = "Permissions.CreateOrders.Edit";
            public const string Delete = "Permissions.CreateOrders.Delete";
        }
        public static class Dashboard
        {
            public const string View = "Permissions.Dashboard.View";
            public const string Create = "Permissions.Dashboard.Create";
            public const string Edit = "Permissions.Dashboard.Edit";
            public const string Delete = "Permissions.Dashboard.Delete";
        }
        public static class Department
        {
            public const string View = "Permissions.Department.View";
            public const string Create = "Permissions.Department.Create";
            public const string Edit = "Permissions.Department.Edit";
            public const string Delete = "Permissions.Department.Delete";
        }
        public static class Home
        {
            public const string View = "Permissions.Home.View";
            public const string Create = "Permissions.Home.Create";
            public const string Edit = "Permissions.Home.Edit";
            public const string Delete = "Permissions.Home.Delete";
        }
        public static class InspectionOrder
        {
            public const string View = "Permissions.InspectionOrder.View";
            public const string Create = "Permissions.InspectionOrder.Create";
            public const string Edit = "Permissions.InspectionOrder.Edit";
            public const string Delete = "Permissions.InspectionOrder.Delete";
        }
        public static class InspectionSetup
        {
            public const string View = "Permissions.InspectionSetup.View";
            public const string Create = "Permissions.InspectionSetup.Create";
            public const string Edit = "Permissions.InspectionSetup.Edit";
            public const string Delete = "Permissions.InspectionSetup.Delete";
        }
        public static class Items
        {
            public const string View = "Permissions.Items.View";
            public const string Create = "Permissions.Items.Create";
            public const string Edit = "Permissions.Items.Edit";
            public const string Delete = "Permissions.Items.Delete";
        }
        public static class Notifications
        {
            public const string View = "Permissions.Notifications.View";
            public const string Create = "Permissions.Notifications.Create";
            public const string Edit = "Permissions.Notifications.Edit";
            public const string Delete = "Permissions.Notifications.Delete";
        }
        public static class Organization
        {
            public const string View = "Permissions.Organization.View";
            public const string Create = "Permissions.Organization.Create";
            public const string Edit = "Permissions.Organization.Edit";
            public const string Delete = "Permissions.Organization.Delete";
        }
        public static class Profile
        {
            public const string View = "Permissions.Profile.View";
            public const string Create = "Permissions.Profile.Create";
            public const string Edit = "Permissions.Profile.Edit";
            public const string Delete = "Permissions.Profile.Delete";
        }
        public static class Report
        {
            //public const string View = "Permissions.Report.View";
            //public const string Create = "Permissions.Report.Create";
            //public const string Edit = "Permissions.Report.Edit";
            //public const string Delete = "Permissions.Report.Delete";
            public const string ReportAnalysis = "Permissions.Report.Analysis";
            public const string ReportOrders = "Permissions.Report.Orders";
            public const string ReportItems = "Permissions.Report.Items";
            public const string ReportInspection = "Permissions.Report.Inspection";

        }
        public static class Role
        {
            public const string View = "Permissions.Role.View";
            public const string Create = "Permissions.Role.Create";
            public const string Edit = "Permissions.Role.Edit";
            public const string Delete = "Permissions.Role.Delete";
        }
        public static class ServiceOrders
        {
            public const string View = "Permissions.ServiceOrders.View";
            public const string Create = "Permissions.ServiceOrders.Create";
            public const string Edit = "Permissions.ServiceOrders.Edit";
            public const string Delete = "Permissions.ServiceOrders.Delete";
        }
        public static class Settings
        {
            public const string View = "Permissions.Settings.View";
            public const string Create = "Permissions.Settings.Create";
            public const string Edit = "Permissions.Settings.Edit";
            public const string Delete = "Permissions.Settings.Delete";
        }
        public static class StaffManagement
        {
            public const string View = "Permissions.StaffManagement.View";
            public const string Create = "Permissions.StaffManagement.Create";
            public const string Edit = "Permissions.StaffManagement.Edit";
            public const string Delete = "Permissions.StaffManagement.Delete";
            public const string AllDepartments = "Permissions.StaffManagement.AllDepartments";
        }
        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }
        //New screen

        public static class Units
        {
            public const string View = "Permissions.Units.View";
            public const string Create = "Permissions.Units.Create";
            public const string Edit = "Permissions.Units.Edit";
            public const string Delete = "Permissions.Units.Delete";
        }
        public static class ShiftWork
        {
            public const string View = "Permissions.ShiftWork.View";
            public const string Create = "Permissions.ShiftWork.Create";
            public const string Edit = "Permissions.ShiftWork.Edit";
            public const string Delete = "Permissions.ShiftWork.Delete";
        }
        public static class SetupRooms
        {
            public const string View = "Permissions.SetupRooms.View";
            public const string Create = "Permissions.SetupRooms.Create";
            public const string Edit = "Permissions.SetupRooms.Edit";
            public const string Delete = "Permissions.SetupRooms.Delete";
        }

        public static class TmTask
        {
            public const string View = "Permissions.TmTask.View";
            public const string Create = "Permissions.TmTask.Create";
            public const string Edit = "Permissions.TmTask.Edit";
            public const string Delete = "Permissions.TmTask.Delete";
        }
    }
}
