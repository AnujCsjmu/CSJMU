using CoreLayout.Controllers;
using CoreLayout.Filters;
using CoreLayout.Models.UserManagement;
using CoreLayout.Repositories.Audit;
using CoreLayout.Repositories.Common;
using CoreLayout.Repositories.Masters.Branch;
using CoreLayout.Repositories.Masters.City;
using CoreLayout.Repositories.Masters.Country;
using CoreLayout.Repositories.Masters.Course;
using CoreLayout.Repositories.Masters.CourseBranchMapping;
using CoreLayout.Repositories.Masters.CourseDetails;
using CoreLayout.Repositories.Masters.Dashboard;
using CoreLayout.Repositories.Masters.Degree;
using CoreLayout.Repositories.Masters.Deparment;
using CoreLayout.Repositories.Masters.District;
using CoreLayout.Repositories.Masters.Faculty;
using CoreLayout.Repositories.Masters.Institute;
using CoreLayout.Repositories.Masters.InstituteCategory;
using CoreLayout.Repositories.Masters.InstituteType;
using CoreLayout.Repositories.Masters.Pratice;
using CoreLayout.Repositories.Masters.Program;
using CoreLayout.Repositories.Masters.Role;
using CoreLayout.Repositories.Masters.State;
using CoreLayout.Repositories.Masters.Tehsil;
using CoreLayout.Repositories.UserManagement.AssignMenuByRole;
using CoreLayout.Repositories.UserManagement.AssignMenuByUser;
using CoreLayout.Repositories.UserManagement.AssignRole;
using CoreLayout.Repositories.UserManagement.ButtonPermission;
using CoreLayout.Repositories.UserManagement.Login;
using CoreLayout.Repositories.UserManagement.Menu;
using CoreLayout.Repositories.UserManagement.ParentMenu;
using CoreLayout.Repositories.UserManagement.Registration;
using CoreLayout.Repositories.UserManagement.SubMenu;
using CoreLayout.Services.Audit;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Branch;
using CoreLayout.Services.Masters.City;
using CoreLayout.Services.Masters.Country;
using CoreLayout.Services.Masters.Course;
using CoreLayout.Services.Masters.CourseBranchMapping;
using CoreLayout.Services.Masters.CourseDetails;
using CoreLayout.Services.Masters.Dashboard;
using CoreLayout.Services.Masters.Degree;
using CoreLayout.Services.Masters.Department;
using CoreLayout.Services.Masters.District;
using CoreLayout.Services.Masters.Faculty;
using CoreLayout.Services.Masters.Institute;
using CoreLayout.Services.Masters.InstituteCategory;
using CoreLayout.Services.Masters.InstituteType;
using CoreLayout.Services.Masters.Pratice;
using CoreLayout.Services.Masters.Program;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.Masters.State;
using CoreLayout.Services.Masters.Tehsil;
using CoreLayout.Services.Registration;
using CoreLayout.Services.UserManagement.AssignMenuByRole;
using CoreLayout.Services.UserManagement.AssignMenuByUser;
using CoreLayout.Services.UserManagement.AssignRole;
using CoreLayout.Services.UserManagement.ButtonPermission;
using CoreLayout.Services.UserManagement.Login;
using CoreLayout.Services.UserManagement.Menu;
using CoreLayout.Services.UserManagement.ParentMenu;
using CoreLayout.Services.UserManagement.SubMenu;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace CoreLayout
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostEnvironment = env;
        }
        public IWebHostEnvironment HostEnvironment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //authorization
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddMvcCore().AddDataAnnotations();

            //start from old projects
            //if (HostEnvironment.IsDevelopment())
            //{
            //    services.AddControllersWithViews().AddRazorRuntimeCompilation().AddNewtonsoftJson(options =>
            //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //    );
            //}
            //else
            //{
            //    services.AddControllersWithViews().AddNewtonsoftJson(options =>
            //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //    );
            //}
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IFileProvider>(
            new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            services.AddMvc(option =>
            {
                option.Filters.Add(typeof(ExceptionLogFilter));
                option.Filters.Add(typeof(AuditFilterAttribute));
            });
            services.AddMvc(options => options.EnableEndpointRouting = false);
            //end


            //session
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(10);//You can set Time   
                options.Cookie.HttpOnly = true;
            });

            //common Site contex
            //services.AddScoped<ISiteContext, SiteContext>();

            //login
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<ILoginService, LoginService>();

            //registration
            services.AddScoped<IRegistrationRepository, RegistrationRepository>();
            services.AddScoped<IRegistrationService, RegistrationService>();

            //Dashboard
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IDashboardService, DashboardService>();

            //country
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICountryService, CountryService>();

            //state
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IStateService, StateService>();

            //city
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICityService, CityService>();

            //District
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<IDistrictService, DistrictService>();

            //Tehsil
            services.AddScoped<ITehsilRepository, TehsilRepository>();
            services.AddScoped<ITehsilService, TehsilService>();

            //menu
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IMenuRepository, MenuRepository>();

            //parent menu
            services.AddScoped<IParentMenuService, ParentMenuService>();
            services.AddScoped<IParentMenuRepository, ParentMenuRepository>();

            //sub menu
            services.AddScoped<ISubMenuService, SubMenuService>();
            services.AddScoped<ISubMenuRepository, SubMenuRepository>();

            //role
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleRepository,RoleRepository>();

            //assign role
            services.AddScoped<IAssignRoleService, AssignRoleService>();
            services.AddScoped<IAssignRoleRepository, AssignRoleRepository>();

            //assign menu by role
            services.AddScoped<IAssignMenuByRoleService, AssignMenuByRoleService>();
            services.AddScoped<IAssignMenuByRoleRepository, AssignMenuByRoleRepository>();

            //assign  menu by user
            services.AddScoped<IAssignMenuByUserService, AssignMenuByUserService>();
            services.AddScoped<IAssignMenuByUserRepository, AssignMenuByUserRepository>();

            //pratice
            services.AddScoped<IPraticeService, PraticeService>();
            services.AddScoped<IPraticeRepository, PraticeRepository>();

            //Button
            services.AddScoped<IButtonService, ButtonService>();
            services.AddScoped<IButtonRepository, ButtonRepository>();

            //ButtonPermission
            services.AddScoped<IButtonPermissionService, ButtonPermissionService>();
            services.AddScoped<IButtonPermissionRepository, ButtonPermissionRepository>();

            //department
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            //common
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<ICommonRepository, CommonRepository>();

            //common controller
            services.AddScoped <CommonController>();
            services.AddScoped<RegistrationModel>();

            //session
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Audit
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<IAuditRepository, AuditRepository>();

            //Institute
            services.AddScoped<IInstituteService, InstituteService>();
            services.AddScoped<IInstituteRepository, InstituteRepository>();

            //Institute Type
            services.AddScoped<IInstituteTypeService, InstituteTypeService>();
            services.AddScoped<IInstituteTypeRepository, InstituteTypeRepository>();

            //Institute Category
            services.AddScoped<IInstituteCategoryService, InstituteCategoryService>();
            services.AddScoped<IInstituteCategoryRepository, InstituteCategoryRepository>();

            //Degree
            services.AddScoped<IDegreeService, DegreeService>();
            services.AddScoped<IDegreeRepository, DegreeRepository>();

            //Program
            services.AddScoped<IProgramService, ProgramService>();
            services.AddScoped<IProgramRepository, ProgramRepository>();

            //Faculty
            services.AddScoped<IFacultyService, FacultyService>();
            services.AddScoped<IFacultyRepository, FacultyRepository>();

            //CourseDetails
            services.AddScoped<ICourseDetailsService, CourseDetailsService>();
            services.AddScoped<ICourseDetailsRepository, CourseDetailsRepository>();

            //CourseDetails
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ICourseRepository, CourseRepository>();

            //BranchDetails
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IBranchRepository, BranchRepository>();

            //BranchDetails
            services.AddScoped<ICourseBranchMappingService, CourseBranchMappingService>();
            services.AddScoped<ICourseBranchMappingRepository, CourseBranchMappingRepository>();

            //add for encrypt value
            services.AddDataProtection();

            //add for ip address
            //services.AddMvc();
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
           
            //new
            app.UseSession();


            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Login}");
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                   name: "Admin",
                   areaName: "Admin",
                   pattern: "Admin/{controller=State}/{action=Index}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");

                endpoints.MapRazorPages();
            });

        }
    }
}
