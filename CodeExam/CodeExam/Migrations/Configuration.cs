namespace CodeExam.Migrations
{
    using CodeExam.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CodeExam.Models.CodeWarDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CodeExam.Models.CodeWarDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.DataTypes.AddOrUpdate(u => u.DataTypeId,
                new DataType { DataTypeName = "integer", DisplayName = "Integer", ExampleDataType = "1" },
                new DataType { DataTypeName = "long", DisplayName = "Long", ExampleDataType = "1" },
                new DataType { DataTypeName = "bool", DisplayName = "Boolean", ExampleDataType = "true" },
                new DataType { DataTypeName = "float", DisplayName = "Float", ExampleDataType = "0.5" },
                new DataType { DataTypeName = "string", DisplayName = "String", ExampleDataType = "\"abc\"" },
                new DataType { DataTypeName = "char", DisplayName = "Char", ExampleDataType = "'a'" },
                new DataType { DataTypeName = "arrayofint", DisplayName = "ArrayOfInt", ExampleDataType = "[1,2]" },
                new DataType { DataTypeName = "arrayoflong", DisplayName = "ArrayOfLong", ExampleDataType = "[1,2]" },
                new DataType { DataTypeName = "arrayofbool", DisplayName = "ArrayOfBool", ExampleDataType = "[true,flase]" },
                new DataType { DataTypeName = "arrayoffloat", DisplayName = "ArrayOfFloat", ExampleDataType = "[1.5,2.5]" },
                new DataType { DataTypeName = "arrayofstring", DisplayName = "ArrayOfString", ExampleDataType = "[\"abc\",\"cde\"]" },
                new DataType { DataTypeName = "arrayofchar", DisplayName = "ArrayOfChar", ExampleDataType = "['a','b']" });

            context.ControllerActions.AddOrUpdate(u => u.CtrlId,
                new ControllerAction { Ctrl = "Home", Area = "Admin", Description = "DashBoard" },
                new ControllerAction { Ctrl = "Task", Description = "Task Manager" },
                new ControllerAction { Ctrl = "User", Description = "User Manager" },
                new ControllerAction { Ctrl = "Role", Description = "Role Manager" },
                new ControllerAction { Ctrl = "Direction", Description = "Direction" });

            context.RoleUsers.AddOrUpdate(u => u.RoleId,
                new RoleUser { RoleName = "Admin", RoleStatus = 1});

            context.Users.AddOrUpdate(u => u.UserId, 
            new User { UserName = "admin", Password = Encryption.Encrypt("admin"), RoleId = 1, UserStatus = 1 });
            
            //context.RoleControllers.AddOrUpdate(u => u.ID,
            //    new RoleController { CtrlId })
        }
    }
}
