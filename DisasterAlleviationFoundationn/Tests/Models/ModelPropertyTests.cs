using DisasterAlleviationFoundationn.Models;
using Xunit;

namespace DisasterAlleviationFoundationn.Tests.Models
{
    public class ModelPropertyTests
    {
        [Fact]
        public void DisasterIncident_DefaultValues_AreSetCorrectly()
        {
            // Arrange & Act
            var incident = new DisasterIncident();

            // Assert
            Assert.Equal(DateTime.Now.Date, incident.OccurredOn.Date);
            Assert.Equal(DateTime.Now.Date, incident.ReportedOn.Date);
            Assert.Equal("1", incident.SeverityLevel);
            Assert.Equal("Reported", incident.Status);
            Assert.Equal(string.Empty, incident.Title);
            Assert.Equal(string.Empty, incident.Description);
        }

        [Fact]
        public void Donation_DefaultValues_AreSetCorrectly()
        {
            // Arrange & Act
            var donation = new Donation();

            // Assert
            Assert.Equal(DateTime.Now.Date, donation.DonatedOn.Date);
            Assert.Equal("Pending", donation.Status);
            Assert.Equal(string.Empty, donation.Type);
            Assert.Equal(string.Empty, donation.Description);
        }

        [Fact]
        public void Volunteer_DefaultValues_AreSetCorrectly()
        {
            // Arrange & Act
            var volunteer = new Volunteer();

            // Assert
            Assert.Equal(DateTime.Now.Date, volunteer.AppliedOn.Date);
            Assert.Equal("Pending", volunteer.Status);
            Assert.Equal(string.Empty, volunteer.Availability);
            Assert.Equal(string.Empty, volunteer.Location);
        }

        [Fact]
        public void User_DefaultValues_AreSetCorrectly()
        {
            // Arrange & Act
            var user = new User();

            // Assert
            Assert.Equal(DateTime.UtcNow.Date, user.CreatedAt.Date);
            Assert.True(user.IsActive);
            Assert.True(user.IsEmailConfirmed);
            Assert.Equal("User", user.Role);
            Assert.Equal(string.Empty, user.FirstName);
            Assert.Equal(string.Empty, user.LastName);
            Assert.Equal(string.Empty, user.Email);
        }

        [Fact]
        public void DisasterIncident_ImageUrl_CanBeNull()
        {
            // Arrange & Act
            var incident = new DisasterIncident
            {
                ImageUrl = null
            };

            // Assert
            Assert.Null(incident.ImageUrl);
        }

        [Fact]
        public void User_OptionalProperties_CanBeNull()
        {
            // Arrange & Act
            var user = new User
            {
                PhoneNumber = null,
                ProfileImage = null,
                Location = null
            };

            // Assert
            Assert.Null(user.PhoneNumber);
            Assert.Null(user.ProfileImage);
            Assert.Null(user.Location);
        }

        [Fact]
        public void Donation_ExpiryDate_CanBeNull()
        {
            // Arrange & Act
            var donation = new Donation
            {
                ExpiryDate = null
            };

            // Assert
            Assert.Null(donation.ExpiryDate);
        }

        [Fact]
        public void Skill_Description_CanBeNull()
        {
            // Arrange & Act
            var skill = new Skill
            {
                Description = null
            };

            // Assert
            Assert.Null(skill.Description);
        }

        [Fact]
        public void DisasterIncident_NavigationProperties_CanBeNull()
        {
            // Arrange & Act
            var incident = new DisasterIncident
            {
                ReportedByUser = null
            };

            // Assert
            Assert.Null(incident.ReportedByUser);
        }

        [Fact]
        public void Donation_NavigationProperties_CanBeNull()
        {
            // Arrange & Act
            var donation = new Donation
            {
                Donor = null,
                AssignedDisaster = null
            };

            // Assert
            Assert.Null(donation.Donor);
            Assert.Null(donation.AssignedDisaster);
        }

        [Fact]
        public void Volunteer_NavigationProperties_CanBeNull()
        {
            // Arrange & Act
            var volunteer = new Volunteer
            {
                User = null,
                Skills = null
            };

            // Assert
            Assert.Null(volunteer.User);
            Assert.Null(volunteer.Skills);
        }

        [Fact]
        public void User_CollectionProperties_CanBeNull()
        {
            // Arrange & Act
            var user = new User
            {
                ReportedIncidents = null,
                Donations = null,
                VolunteerApplications = null
            };

            // Assert
            Assert.Null(user.ReportedIncidents);
            Assert.Null(user.Donations);
            Assert.Null(user.VolunteerApplications);
        }
    }
}
