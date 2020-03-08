
// Login and resigistration from with Session login.
//===============================================================================


public class AccountController : Controller
    {
        RealEstateDBEntities1 db = new RealEstateDBEntities1();
        // GET: Account
       
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(UserTB usertable)
        {  
            db.UserTBs.Add(usertable);
            db.SaveChanges();
            ViewBag.Successfulmessage = "You are successfully Register"; 
            return View();
        }
       
        public ActionResult Login()
        {
            if (Session["Name"] != null)
            {
                return RedirectToAction("mainPage", "Account", new { username = Session["Name"].ToString() });            
            }
            else
            {
                return View();  
            }
            
        }

        [HttpPost]
        public ActionResult Login(UserTB usertable)
        { 
               var userLoggedIn = db.UserTBs.Where(m => m.user_name == usertable.user_name && m.user_password == usertable.user_password).FirstOrDefault();
                if (userLoggedIn != null) 
                {
                     ViewBag.message = "loggedin";
                     ViewBag.tryiedOne = "Yes";
                    Session["ID"] = usertable.user_id.ToString();
                    Session["Name"] = usertable.user_name.ToString();
                    return RedirectToAction("mainPage", "Account", new { username= usertable.user_name});                }
                else
                {
                FormsAuthentication.SetAuthCookie(usertable.user_name, false);
                ViewBag.triedOne = "Yes";
                ViewBag.notlogin = "your user id and password do not match";
                return View();
                } 
        }      

        public ActionResult mainPage(string username)
        {
            if (Session["Name"] == null)
            {
                return RedirectToAction("Login","Account");
            }
            else
            {
                ViewBag.username = Session["Name"];
                return View(); 
            } 
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Accounts");
        }
    }

    //========================================================================================================================

    Registration.cshtml

     <h1>@ViewBag.Successfulmessage</h1>
    @using (Html.BeginForm("Registration", "Account", FormMethod.Post))
    {
        <div class="form-group">
            <div>
                <div> <label for="your_name"><i class="zmdi zmdi-account material-icons-name"></i></label></div>
                <div> @Html.EditorFor(model => model.user_name, new { htmlAttributes = new { @class = "form-control", @placeholder = "User Name" } })</div>
                <div> @Html.ValidationMessageFor(model => model.user_name, "", new { @class = "text-danger" })</div>
            </div>
        </div> 

        <div class="signup-image-link">
        @Html.ActionLink("I am already member", "login", "Account")
        </div>
    }


//========================================================================================================================

   login.cshtml

    <h1>@ViewBag.notlogin</h1>
    @using (Html.BeginForm("Login", "Account", FormMethod.Post))
    {
        <div class="signup-image-link">
        @Html.ActionLink("I am already member", "login", "Account")
        </div>
    } 
 
 //========================================================================================================================
   