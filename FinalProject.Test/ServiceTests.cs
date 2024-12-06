
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
                Description = "An event for dos to socialise",
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
        public void SearchEvents_WithMatchingQuery_ShouldReturnEvents()
        {
            // Arrange
            eventService.AddEvent(new Event 
            {
                Title = "Agility Training",
                EventTime = DateTime.Now.AddDays(1),
                Location = "Omagh Community Dog Park",
                Description = "Training session for agility.",
                ImageUrl = "/images/agility-training.png"
            });

            eventService.AddEvent(new Event
            {
                Title = "Collie MeetUp",
                EventTime = DateTime.Now.AddDays(20),
                Location = "Grange Park, Omagh",
                Description = "Playful Collie meetup.",
                ImageUrl = "/images/collie-meetup.png"
            });

            // Act 
            var matchingEvents = eventService.SearchEvents("Agility").ToList();

            // Debugging: log the results
            Console.WriteLine($"Found {matchingEvents.Count} matching events.");
            foreach (var evt in matchingEvents)
            {
                Console.WriteLine($"Event: {evt.Title}, Location: {evt.Location}");
            }

            // Assert
            Assert.Single(matchingEvents);
            Assert.Equal("Agility Training", matchingEvents[0].Title);
        }
    }
}
