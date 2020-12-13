using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Task_Management_Platform.Models;

[assembly: OwinStartupAttribute(typeof(Task_Management_Platform.Startup))]
namespace Task_Management_Platform
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateAdminUserAndApplicationRoles();
        }

        private void CreateAdminUserAndApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Se adauga rolurile aplicatiei
            //Admin (administratorul aplicatiei)
            //are acces peste tot si poate face orice modifcari doreste, 
            //inclisiv sa revoce sau sa adauge deprturi altor utilizatori
            if (!roleManager.RoleExists("Admin"))
            {
                // Se adauga rolul de administrator
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                // se adauga utilizatorul administrator
                var user = new ApplicationUser();
                user.UserName = "admin@admin.com";
                user.Email = "admin@admin.com";
                var adminCreated = UserManager.Create(user, "1!Admin");
                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Admin");
                }
            }

            //Organizator (utilizator inregistrat ce a creat o echipa) 
            //are dreptul de a dauga persoane in echipe si de a crea/edita taskuri/comentarii/proiecte
            if (!roleManager.RoleExists("Organizator"))
            {
                var role = new IdentityRole();
                role.Name = "Organizator";
                roleManager.Create(role);
            }

            //Membru (utilizator inregistrat ce face parte din cel putin o echipa)
            //are dreptul de a lasa comentarii la taskuri si de a edita/sterge comenatriile proprii
            if (!roleManager.RoleExists("Membru"))
            {
                var role = new IdentityRole();
                role.Name = "Membru";
                roleManager.Create(role);
            }

            //User (utilizator inregistrat care nu face parte din nici o echipa)
            //poate vizualiza echipele si poate crea echipa sau sa fie adaugat intr-una
            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }
        }
    }
}
