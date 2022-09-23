using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManagment.Data
{
    /// <summary>
    /// Provides methods for filling the database with demo data
    /// </summary>
    public class DemoDataGenerator
    {
        private readonly UserManagmentDataContext context;

        public DemoDataGenerator(UserManagmentDataContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Delete all data in the database
        /// </summary>
        /// <returns></returns>
        public async Task ClearAll()
        {
            context.Users.RemoveRange(await context.Users.ToArrayAsync());
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Fill database with demo data
        /// </summary>
        public async Task Fill()
        {
            #region Add some users
          
            User foo, john, jane;

            context.Users.Add(foo = new User
            {
                NameIdentifier = "foo.bar",
                FirstName = "Foo",
                LastName = "Bar",
                Email = "foo.bar@acme.corp"
            });

            context.Users.Add(jane = new User
            {
                NameIdentifier = "jane.doe",
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@acme.corp"
            });
            context.Users.Add(john = new User
            {
                NameIdentifier = "john.doe",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@acme.corp"
            });
            #endregion


            #region Add some groups
            // Add code to generate demo groups here
            Group g1,g2,g3;
            context.Groups.Add(g1 = new Group
            {
                Name = "Group 1",
                Users = new() { john, jane }
            });
            context.Groups.Add(g2 = new Group
            {
                Name = "Group 1",
                Users = new() { foo, jane }
            });
            context.Groups.Add(g3 = new Group
            {
                Name = "Group 1",
                Users = new() { john }
            });
            #endregion

            await context.SaveChangesAsync();
        }
    }
}
