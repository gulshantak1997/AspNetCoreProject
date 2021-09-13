using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using AdminLTE1.Helper;
using AdminLTE1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace AdminLTE1.Controllers
{
    public class FinalProjectController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FinalProjectController(AppDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MeetingCalendar()
        {
            IEnumerable<Users> userModel = (from user in _context.Users
                                            select new Users()
                                            {
                                                UserId = user.Id,
                                                UserName = user.UserName
                                            }).ToList();

            return View(userModel);
        }

        public JsonResult GetEvents()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var headUser = (from huser in _context.Head select huser.UserId).FirstOrDefault();

            var allEvents = new List<Event>();

            if (userId == headUser)
            {
                allEvents = _context.Event.ToList();

            }
            else
            {
                var particularAttendee = _context.Attendees.Where(a => a.AttendeeId == Guid.Parse(userId)).ToList();

                foreach (var item in particularAttendee)
                {
                    var newallEvents = _context.Event.Where(a => a.Id == item.EventId).ToList();
                    allEvents.AddRange(newallEvents);
                }

            }

            return Json(allEvents, new JsonSerializerSettings());
        }


        [HttpPost]
        public JsonResult SaveEvent(Event eventObj, List<Guid> attendeeObj)
        {
            var status = false;

            if (eventObj.Id > 0)
            {
                //Update the event
                var eventDetail = _context.Event.Where(a => a.Id == eventObj.Id).FirstOrDefault();


                if (eventDetail != null)
                {
                    eventDetail.Subject = eventObj.Subject;
                    eventDetail.StartDate = eventObj.StartDate;
                    eventDetail.EndDate = eventObj.EndDate;
                    eventDetail.Description = eventObj.Description;
                    eventDetail.IsAllDayEvent = eventObj.IsAllDayEvent;
                }
                _context.SaveChanges();


                var attendeeDetail = _context.Attendees.Where(a => a.EventId == eventObj.Id).ToList();
                if (attendeeDetail != null)
                {
                    foreach (var item in attendeeDetail)
                    {
                        _context.Attendees.Remove(item);
                        _context.SaveChanges();
                    }

                }

                foreach (var item in attendeeObj)
                {
                    Attendees atteObj = new Attendees();
                    atteObj.EventId = eventObj.Id;
                    atteObj.AttendeeId = item;
                    _context.Attendees.Add(atteObj);
                    _context.SaveChanges();
                }

            }
            else
            {
                var eventItem = _context.Event.Add(eventObj);
                _context.SaveChanges();

                foreach (var item in attendeeObj)
                {
                    Attendees atteObj = new Attendees();
                    atteObj.AttendeeId = item;
                    atteObj.EventId = eventItem.Entity.Id;
                    _context.Attendees.Add(atteObj);
                    _context.SaveChanges();
                }
            }

            var listUser = _context.Users.Where(a => attendeeObj.Contains(Guid.Parse(a.Id))).ToList();

            foreach (var item in listUser)
            {
                var zoomUrl = "https://www.google.com/";
                string wwwRootPath = _hostingEnvironment.WebRootPath;
                string filePath = Path.Combine(wwwRootPath, "EmailTemplate", "NewMeetingTemplate.html");


                var htmlContent = System.IO.File.ReadAllText(filePath);
                htmlContent = htmlContent.Replace("[UserName]", item.FirstName + " " + item.LastName);
                htmlContent = htmlContent.Replace("[Zoom_URL]", zoomUrl);

                SendEmail(item.Email, htmlContent);
            }

            status = true;

            return Json(new { status = status });
        }

        public bool SendEmail(string email, string htmlMessage)
        {
            string fromMail = "gulshantak1997@gmail.com";
            string fromPassword = "dsoe xoae kfxy orkz";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.To.Add(new MailAddress(email));
            message.Body = "<html><body> " + htmlMessage + " </body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,

            };
            smtpClient.Send(message);

            return true;
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            var eventDetail = _context.Event.Where(a => a.Id == eventID).FirstOrDefault();
            if (eventDetail != null)
            {
                _context.Event.Remove(eventDetail);
                _context.SaveChanges();

            }

            var attendeeDetail = _context.Attendees.Where(a => a.EventId == eventID).ToList();
            if (attendeeDetail != null)
            {
                foreach (var item in attendeeDetail)
                {
                    _context.Attendees.Remove(item);
                    _context.SaveChanges();
                }

                status = true;
            }

            return Json(new { status = status });
        }


        public JsonResult GetInviteIds(int eventId)
        {
            var attendeesId = (from attendee in _context.Attendees.Where(r => r.EventId == eventId)
                               select new SelectListItem
                               {
                                   Value = Convert.ToString(attendee.AttendeeId)
                               }).ToList();

            return Json(attendeesId);
        }

        [HttpGet]
        public JsonResult LoggedUserNHeadUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var headUser = (from huser in _context.Head select huser.UserId).FirstOrDefault();
            var checkUser = false;
            if (userId == headUser)
            {
                checkUser = true;
            }

            return Json(new { checkUser = checkUser });
        }



        public IActionResult LeaveCalendar()
        {
            IEnumerable<Users> userModel = (from user in _context.Users
                                            select new Users()
                                            {
                                                UserId = user.Id,
                                                UserName = user.UserName
                                            }).ToList();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.userId = userId;
            var headUser = (from user in _context.Head select user.UserId).FirstOrDefault();
            ViewBag.headUser = headUser;

            return View(userModel);
        }


        public JsonResult GetLeaveEvent()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var headUser = (from huser in _context.Head select huser.UserId).FirstOrDefault();

            var allEvents = new List<Leave>();
            if (userId == headUser)
            {
                allEvents = _context.Leave.ToList();
            }
            else
            {
                var particularLeaveUser = _context.Leave.Where(a => a.UserId == userId).ToList();

                foreach (var item in particularLeaveUser)
                {
                    var newallEvents = _context.Leave.Where(a => a.UserId == item.UserId).ToList();
                    allEvents.AddRange(newallEvents);
                }
            }

            return Json(allEvents, new JsonSerializerSettings());
        }


        [HttpPost]
        public JsonResult SaveLeaveEvent(Leave eventObj)
        {
            var status = false;

            if (eventObj.Id > 0)
            {
                //Update the event
                var leaveDetail = _context.Leave.Where(a => a.Id == eventObj.Id).FirstOrDefault();

                if (leaveDetail != null)
                {
                    leaveDetail.StartDate = eventObj.StartDate;
                    leaveDetail.EndDate = eventObj.EndDate;
                    leaveDetail.IsHalfDay = eventObj.IsHalfDay;
                    leaveDetail.Reason = eventObj.Reason;
                    leaveDetail.HandOverPId = eventObj.HandOverPId;
                    leaveDetail.Description = eventObj.Description;
                }
                _context.SaveChanges();
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var headUser = (from huser in _context.Head select huser.UserId).FirstOrDefault();

                var leave = new Leave();
                leave.UserId = userId;
                leave.StartDate = eventObj.StartDate;
                leave.EndDate = eventObj.EndDate;
                leave.IsHalfDay = eventObj.IsHalfDay;
                leave.Reason = eventObj.Reason;
                leave.HandOverPId = eventObj.HandOverPId;
                leave.Description = eventObj.Description;
                leave.Status = "Pending";

                var leaveStore = _context.Leave.Add(leave);
                _context.SaveChanges();

                Head headObj = new Head();
                headObj.UserId = headUser;
                headObj.LeaveUId = userId;
                headObj.LeaveEventId = leaveStore.Entity.Id;
                _context.Head.Add(headObj);
                _context.SaveChanges();

            }

            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var leaveUserDetail = _context.Users.Where(a => a.Id == userID).FirstOrDefault();
            var handOverPDetail = _context.Users.Where(a => a.Id == eventObj.HandOverPId).FirstOrDefault();

            var wwwRootPath = _hostingEnvironment.WebRootPath;
            var filePath = Path.Combine(wwwRootPath, "EmailTemplate", "LeaveTemplate.html");
            var htmlContent = System.IO.File.ReadAllText(filePath);
            htmlContent = htmlContent.Replace("[LeaveUser]", leaveUserDetail.FirstName + " " + leaveUserDetail.LastName);
            htmlContent = htmlContent.Replace("[StartDate]", String.Format("{0:dd/MM/yyyy}", eventObj.StartDate));
            htmlContent = htmlContent.Replace("[EndDate]", String.Format("{0:dd/MM/yyyy}", eventObj.EndDate));
            htmlContent = htmlContent.Replace("[HandOverDescription]", eventObj.Description);
            htmlContent = htmlContent.Replace("[HandOverPName]", handOverPDetail.FirstName + " " + handOverPDetail.LastName);

            var handOverPEmail = _context.Users.Where(a => a.Id == eventObj.HandOverPId).Select(a => a.Email).FirstOrDefault();
            //var handOverPEmail = (from user in _context.Users.Where(a=>a.Id==eventObj.HandOverPId) select user.Email).FirstOrDefault();
            var leaveUserEmail = _context.Users.Where(a => a.Id == userID).Select(a => a.Email).FirstOrDefault();

            SendEmail(leaveUserEmail, htmlContent);
            SendEmail(handOverPEmail, htmlContent);

            status = true;

            return Json(new
            {
                status = status
            });

        }

        public JsonResult GetHandOverPId(int eventId)
        {
            //var handOverPId = from person in _context.Leave.Where(r => r.Id == eventId).Select(r => r.HandOverPId).ToString();
            var handOverPId = from person in _context.Leave where person.Id == eventId select person.HandOverPId;

            return Json(handOverPId);
        }

        public JsonResult UserFirstLastName(int eventId)
        {
            var handOverPName = (from user in _context.Users
                                 join handOverUser in _context.Leave.Where(r => r.Id == eventId)
                                 on user.Id equals handOverUser.HandOverPId
                                 select new Users
                                 {
                                     FirstName = user.FirstName,
                                     LastName = user.LastName
                                 }).ToList();

            var leaveUserName = (from user in _context.Users
                                 join leaveUser in _context.Leave.Where(r => r.Id == eventId)
                                 on user.Id equals leaveUser.UserId
                                 select new Users
                                 {
                                     FirstName = user.FirstName,
                                     LastName = user.LastName
                                 }).ToList();

            var handOverFirstLastName = "";
            var leaveUserFirstLastName = "";
            foreach (var item in handOverPName)
            {
                handOverFirstLastName = item.FirstName + " " + item.LastName;

            }

            foreach (var item in leaveUserName)
            {
                leaveUserFirstLastName = item.FirstName + " " + item.LastName;

            }

            return Json(new { handOverFirstLastName=handOverFirstLastName, leaveUserFirstLastName=leaveUserFirstLastName});
        }



        [HttpPost]
        public JsonResult DeleteLeave(int eventID)
        {
            var status = false;

            var leaveDetail = _context.Leave.Where(a => a.Id == eventID).FirstOrDefault();
            if (leaveDetail != null)
            {
                _context.Leave.Remove(leaveDetail);
                _context.SaveChanges();
            }

            var leaveRemove = _context.Head.Where(a => a.LeaveEventId == eventID).FirstOrDefault();
            _context.Head.Remove(leaveRemove);
            _context.SaveChanges();

            status = true;
            return Json(new { status = status });
        }


        public JsonResult ApproveLeave(int eventID)
        {
            var status = false;
            var particularLeave = _context.Leave.Where(a => a.Id == eventID).FirstOrDefault();

            if (particularLeave != null)
            {
                particularLeave.Status = "Approved";
            }
            _context.SaveChanges();
            status = true;

            return Json(new { status = status });
        }

        public JsonResult UnApproveLeave(int eventID)
        {
            var status = false;
            var particularLeave = _context.Leave.Where(a => a.Id == eventID).FirstOrDefault();

            if (particularLeave != null)
            {
                particularLeave.Status = "UnApproved";

            }
            _context.SaveChanges();
            status = true;

            return Json(new { status = status });
        }

    }
}
