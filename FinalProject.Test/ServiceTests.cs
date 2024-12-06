
using Xunit;
using FinalProject.Data.Entities;
using FinalProject.Data.Services;

using Microsoft.EntityFrameworkCore;
using FinalProject.Data.Repositories;
using FinalProject.Data.Security;

namespace FinalProject.Test
{
    public class ServiceTests
    {
        private readonly IUserService userService;
        private readonly IEventService eventService;
        private readonly ICalendarService calendarService;
        private readonly IPostService postService;

        public ServiceTests()
        {
            // configure the data context options to use sqlite for testing
            var options = DatabaseContext.OptionsBuilder                            
                            .UseSqlite("Filename=test.db")
                            //.LogTo(Console.WriteLine)
                            .Options;

            // create service with new context
            var dbContext = new DatabaseContext(options);
            userService = new UserServiceDb(dbContext);
            eventService = new EventServiceDb(dbContext);
            calendarService = new CalendarServiceDb(dbContext);
            postService = new PostServiceDb(dbContext);

            dbContext.Initialise();
        }

        //==================================== USER SERVICE TESTS ====================================
         [Fact]
        public void GetUsers_WhenNoneExist_ShouldReturnNone()
        {
            // act
            var pagedUsers = userService.GetUsers();

            // assert
            Assert.Empty(pagedUsers.Data);
        }
        
        [Fact]
        public void AddUser_When2ValidUsersAdded_ShouldCreate2Users()
        {
            // arrange
            userService.AddUser("admin", "admin@mail.com", "admin", Role.admin, "Golden Retriever", "/images/admin.png");
            userService.AddUser("guest", "guest@mail.com", "guest", Role.guest, "Beagle", "/images/guest.png");

            // act
            var pagedUsers = userService.GetUsers();

            // assert
            Assert.Equal(2, pagedUsers.Data.Count);
        }

        [Fact]
        public void GetPage1WithpageSize2_When3UsersExist_ShouldReturn2Pages()
        {
            // act
            userService.AddUser("admin", "admin@mail.com", "admin", Role.admin, "Golden Retriever", "/images/admin.png");
            userService.AddUser("manager", "manager@mail.com", "manager", Role.manager, "Labrador", "/images/manager.png");
            userService.AddUser("guest", "guest@mail.com", "guest", Role.guest, "Beagle", "/images/guest.png");

            // return first page with 2 users per page
            var pagedUsers = userService.GetUsers(1,2);

            // assert
            Assert.Equal(2, pagedUsers.TotalPages);
        }

        [Fact]
        public void GetPage1WithPageSize2_When3UsersExist_ShouldReturnPageWith2Users()
        {
            // act
            userService.AddUser("admin", "admin@mail.com", "admin", Role.admin, "Golden Retriever", "/images/admin.png");
            userService.AddUser("manager", "manager@mail.com", "manager", Role.manager, "Labrador", "/images/manager.png");
            userService.AddUser("guest", "guest@mail.com", "guest", Role.guest, "Beagle", "/images/guest.png");

            var pagedUsers = userService.GetUsers(1,2);

            // assert
            Assert.Equal(2, pagedUsers.Data.Count);
        }

        [Fact]
        public void GetPage1_When0UsersExist_ShouldReturn0Pages()
        {
            // act
            var pagedUsers = userService.GetUsers(1,2);

            // assert
            Assert.Equal(0, pagedUsers.TotalPages);
            Assert.Equal(0, pagedUsers.TotalRows);
            Assert.Empty(pagedUsers.Data);
        }

        [Fact]
        public void UpdateUser_WhenUserExists_ShouldWork()
        {
            // arrange
            var user = userService.AddUser("admin", "admin@mail.com", "admin", Role.admin, "Golden Retriever", "/images/admin.png");
            
            // act
            user.Name = "administrator";
            user.Email = "admin@mail.com";            
            var updatedUser = userService.UpdateUser(user);

            // assert
            Assert.Equal("administrator", updatedUser.Name);
            Assert.Equal("admin@mail.com", updatedUser.Email);
        }

        [Fact]
        public void Login_WithValidCredentials_ShouldWork()
        {
            // arrange
            userService.AddUser("admin", "admin@mail.com", "admin", Role.admin, "Golden Retriever", "/images/admin.png");
            
            // act            
            var user = userService.Authenticate("admin@mail.com","admin");

            // assert
            Assert.NotNull(user);
           
        }

        [Fact]
        public void Login_WithInvalidCredentials_ShouldNotWork()
        {
            // arrange
            userService.AddUser("admin", "admin@mail.com", "admin", Role.admin, "Golden Retriever", "/images/admin.png");

            // act      
            var user = userService.Authenticate("admin@mail.com","xxx");

            // assert
            Assert.Null(user);
           
        }

        [Fact]
        public void ForgotPasswordRequest_ForValidUser_ShouldGenerateToken()
        {
            // arrange
            userService.AddUser("admin", "admin@mail.com", "admin", Role.admin, "Golden Retriever", "/images/admin.png");

            // act      
            var token = userService.ForgotPassword("admin@mail.com");

            // assert
            Assert.NotNull(token);
           
        }

        [Fact]
        public void ForgotPasswordRequest_ForInValidUser_ShouldReturnNull()
        {
            // arrange
          
            // act      
            var token = userService.ForgotPassword("admin@mail.com");

            // assert
            Assert.Null(token);
           
        }

        [Fact]
        public void ResetPasswordRequest_WithValidUserAndToken_ShouldReturnUser()
        {
            // arrange
            userService.AddUser("admin", "admin@mail.com", "admin", Role.admin, "Golden Retriever", "/images/admin.png");
            var token = userService.ForgotPassword("admin@mail.com");
            
            // act      
            var user = userService.ResetPassword("admin@mail.com", token, "password");
        
            // assert
            Assert.NotNull(user);
            Assert.True(Hasher.ValidateHash(user.Password, "password"));          
        }

        [Fact]
        public void ResetPasswordRequest_WithValidUserAndExpiredToken_ShouldReturnNull()
        {
            // arrange
            userService.AddUser("admin", "admin@mail.com", "admin", Role.admin, "Golden Retriever", "/images/admin.png");
            var expiredToken = userService.ForgotPassword("admin@mail.com");
            userService.ForgotPassword("admin@mail.com");
            
            // act      
            var user = userService.ResetPassword("admin@mail.com", expiredToken, "password");
        
            // assert
            Assert.Null(user);  
        }

        [Fact]
        public void ResetPasswordRequest_WithInValidUserAndValidToken_ShouldReturnNull()
        {
            // arrange
            userService.AddUser("admin", "admin@mail.com", "admin", Role.admin, "Golden Retriever", "/images/admin.png");          
            var token = userService.ForgotPassword("admin@mail.com");
            
            // act      
            var user = userService.ResetPassword("unknown@mail.com", token, "password");
        
            // assert
            Assert.Null(user);  
        }

        [Fact]
        public void ResetPasswordRequests_WhenAllCompleted_ShouldExpireAllTokens()
        {
            // arrange
            userService.AddUser("admin", "admin@mail.com", "admin", Role.admin, "Golden Retriever", "/images/admin.png");       
            userService.AddUser("guest", "guest@mail.com", "guest", Role.guest, "Beagle", "/images/guest.png");          

            // create token and reset password - token then invalidated
            var token1 = userService.ForgotPassword("admin@mail.com");
            userService.ResetPassword("admin@mail.com", token1, "password");

            // create token and reset password - token then invalidated
            var token2 = userService.ForgotPassword("guest@mail.com");
            userService.ResetPassword("guest@mail.com", token2, "password");
         
            // act  
            // retrieve valid tokens 
            var tokens = userService.GetValidPasswordResetTokens();   

            // assert
            Assert.Empty(tokens);
        }

        //==================================== EVENT SERVICE TESTS ====================================
        [Fact]
        public void GetEvents_WhenNoneExist_ShouldReturnNone()
        {
            // Act
            var pagedEvents = eventService.GetEvents();

            // Assert
            Assert.Empty(pagedEvents.Data);
        }

        [Fact]
        public void AddEvent_WhenValidEventAdded_ShouldCreateEvent()
        {
            // Arrange
            var newEvent = new Event
            {
                Title = "Dog Meetup",
                EventTime = DateTime.Now.AddDays(5),
                Location = "Belfast City Hall",
                Description = "A fun meetup for dog owners and their furry friends.",
                ImageUrl = "/images/dog-meetup.png"
            };

            // Act
            var addedEvent = eventService.AddEvent(newEvent);

            // Assert
            Assert.NotNull(addedEvent);
            Assert.Equal("Dog Meetup", addedEvent.Title);
        }

        [Fact]
        public void GetEventsById_WhenEventExists_ShouldReturnEvent()
        {
            // Arrange
            var addedEvent = eventService.AddEvent(new Event 
            {
                Title = "Puppy Training",
                EventTime = DateTime.Now.AddDays(10),
                Location = "Omagh Leisure Center",
                Description = "Training for puppies.",
                ImageUrl = "/images/puppy-training.png"
            });

            // Act
            var retrievedEvent = eventService.GetEventById(addedEvent.Id);

            // Assert
            Assert.NotNull(retrievedEvent);
            Assert.Equal("Puppy Training", retrievedEvent.Title);
        }

        [Fact]
        public void DeleteEvent_WhenEventExists_ShouldRemoveEvent()
        {
            // Arrange
            var addedEvent = eventService.AddEvent(new Event {
                Title = "Obedience Class",
                EventTime = DateTime.Now.AddDays(7),
                Location = "Carrickmore Youth Club",
                Description = "A class for training obedient dogs.",
                ImageUrl = "/images/obedience-class.png"
            });

            // Act
            var isDeleted = eventService.DeleteEvent(addedEvent.Id);

            // Assert
            Assert.True(isDeleted);
            Assert.Null(eventService.GetEventById(addedEvent.Id));
        }

        [Fact]
        public void UpdateEvent_WhenEventExists_ShouldUpdateDetails()
        {
            // Arrange
            var addedEvent = eventService.AddEvent(new Event {
                Title = "Socialising Event",
                EventTime = DateTime.Now.AddDays(13),
                Location = "Wild River Dog Park, Lisburn",
                Description = "An event for dogs to socialise",
                ImageUrl = "/images/socialising-event.png"
            });

            // Act
            addedEvent.Title = "Dog Socialising Event";
            addedEvent.Location = "Wild River Dog Park, Ballynahinch";
            var updatedEvent = eventService.UpdateEvent(addedEvent);

            // Assert
            Assert.NotNull(updatedEvent);
            Assert.Equal("Dog Socialising Event", updatedEvent.Title);
            Assert.Equal("Wild River Dog Park, Ballynahinch", updatedEvent.Location);
        }

        [Fact]
        public void LikeEvent_WhenCalled_ShouldIncrementLikes()
        {
            // Arrange
            var addedEvent = eventService.AddEvent(new Event
            {
                Title = "Collie Meetup",
                EventTime = DateTime.Now.AddDays(5),
                Location = "Park Eire Og, Carrickmore",
                Description = "A fun meetup for collies.",
                ImageUrl = "/images/collie-meetup.png"
            });

            // Act
            var isLiked = eventService.LikeEvent(addedEvent.Id);

            //Assert
            Assert.True(isLiked);
            Assert.Equal(1, eventService.GetEventById(addedEvent.Id).Likes);
        }

        [Fact]
        public void ToggleLike_WhenCalled_ShouldToggleLikeStatus()
        {
            //Arrange
            var userId = "user123";
            var addedEvent = eventService.AddEvent(new Event 
            {
                Title = "Obstacle Course",
                EventTime = DateTime.Now.AddDays(3),
                Location = "Wild River Dog Park, Lisburn",
                Description = "An obstacle event for dogs",
                ImageUrl = "/images/socialising-event.png"
            });

            //Act
            var result1 = eventService.ToggleLike(addedEvent.Id, userId); //First toggle (like)
            var result2 = eventService.ToggleLike(addedEvent.Id, userId); //Second toggle (unlike)

            //Assert
            Assert.True(result1.Liked);
            Assert.False(result2.Liked);
            Assert.Equal(0, result2.Likes);
        }

        [Fact]
        public void CreateReview_WhenValidEvent_ShouldAddReview()
        {
            // Arrange
            var addedEvent = eventService.AddEvent(new Event
            {
                Title = "Dog Walk - All Breeds",
                EventTime = DateTime.Now.AddDays(23),
                Location = "An Creggan Centre, Creggan",
                Description = "An event for dogs to socialise",
                ImageUrl = "/images/dog-walk-event.png"
            });

            //Act 
            var review = eventService.CreateReview(addedEvent.Id, "John", "Great event!", 5);

            //Assert
            Assert.NotNull(review);
            Assert.Equal("John", review.Name);
            Assert.Equal("Great event!", review.Comment);
        }

        //==================================== CALENDAR SERVICE TESTS ====================================
        [Fact]
        public void GetCalendars_WhenNoneExist_ShouldReturnEmptyList()
        {
            //Act
            var calendars = calendarService.GetCalendars();

            //Assert
            Assert.Empty(calendars);
        }

        [Fact]
        public void AddCalendar_WhenValidCalendarAdded_ShouldCreateCalendar()
        {
            //Arrange
            var newCalendar = new Calendar
            {
                Title = "Dog Training Session",
                Location = "Omagh Dog Park",
                Start = DateTime.Now.AddDays(1),
                End = DateTime.Now.AddDays(1).AddHours(2),
                CountyId = 1,
                UserId = 1
            };

            //Act
            var addedCalendar = calendarService.AddCalendar(newCalendar);

            //Assert
            Assert.NotNull(addedCalendar);
            Assert.Equal("Dog Training Session", addedCalendar.Title);
        }

        [Fact]
        public void GetCalendarsByCounty_WhenCalendarsExist_ShouldReturnMatchingCalendars()
        {
            //Arrange
            calendarService.AddCalendar(new Calendar
            {
                Title = "Agility Training",
                Location = "Derry Dog Park",
                Start = DateTime.Now.AddDays(3),
                End = DateTime.Now.AddDays(3).AddHours(1),
                CountyId = 2,
                UserId = 1
            });

            calendarService.AddCalendar(new Calendar
            {
                Title = "Social Dog Walk",
                Location = "Ebrington, Derry",
                Start = DateTime.Now.AddDays(5),
                End = DateTime.Now.AddDays(5).AddHours(2),
                CountyId = 2,
                UserId = 2
            });

            //Act 
            var calendars = calendarService.GetCalendarsByCounty(2);

            //Assert
            Assert.Equal(2, calendars.Count);
        }

        [Fact]
        public void AddCalendar_WhenOverlappingDates_ShouldReturnNull()
        {
            //Arrange
            calendarService.AddCalendar(new Calendar
            {
                Title = "Existing Event",
                Location = "Park A",
                Start = DateTime.Now.AddDays(2),
                End = DateTime.Now.AddDays(2).AddHours(1),
                CountyId = 1,
                UserId = 1
            });

            var overlappingCalendar = new Calendar
            {
                Title = "Overlapping Event",
                Location = "Park B",
                Start = DateTime.Now.AddDays(2).AddMinutes(30),
                End = DateTime.Now.AddDays(2).AddHours(2),
                CountyId = 1,
                UserId = 2
            };

            //Act
            var result = calendarService.AddCalendar(overlappingCalendar);
        }

        [Fact]
        public void DeleteCalendar_WhenCalendarExists_ShouldDeleteSuccessfully()
        {
            //Arrange
            var calendar = calendarService.AddCalendar(new Calendar
            {
                Title = "Delete Me",
                Location = "Anywhere",
                Start = DateTime.Now.AddDays(3),
                End = DateTime.Now.AddDays(3).AddHours(2),
                CountyId = 1,
                UserId = 1
            });

            //Act
            var isDeleted = calendarService.DeleteCalendar(calendar.Id);

            //Assert
            Assert.True(isDeleted);
            Assert.Null(calendarService.GetCalendar(calendar.Id));
        }

        [Fact]
        public void UpdateCalendar_WhenValidUpdate_ShouldSaveChanges()
        {
            //Arrange
            var calendar = calendarService.AddCalendar(new Calendar
            {
                Title = "Old Title",
                Location = "Old Location",
                Start = DateTime.Now.AddDays(1),
                End = DateTime.Now.AddDays(1).AddHours(1),
                CountyId = 1,
                UserId = 1
            });

            //Act
            calendar.Title = "New Title";
            calendar.Location = "New Location";
            var updatedCalendar = calendarService.UpdateCalendar(calendar);

            //Assert
            Assert.NotNull(updatedCalendar);
            Assert.Equal("New Title", updatedCalendar.Title);
            Assert.Equal("New Location", updatedCalendar.Location);
        }

        //==================================== POST SERVICE TESTS ====================================
        [Fact]
        public void GetPosts_WhenNoneExist_ShouldReturnEmptyList()
        {
            //Act
            var posts = postService.GetPosts();

            //Assert
            Assert.Empty(posts.Data);
        }
    }
}
