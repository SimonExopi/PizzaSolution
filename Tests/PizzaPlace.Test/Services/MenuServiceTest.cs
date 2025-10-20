using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzaPlace;
using PizzaPlace.Services;
using System;
using System.Linq;

namespace PizzaPlace.Test.Services
{
    [TestClass]
    public class MenuServiceTest
    {
        private static MenuService GetService() => new MenuService();

        [TestMethod]
        public void GetMenu_ReturnsLunchMenuDuringLunchHours()
        {
            // Arrange
            var svc = GetService();
            // Hour 12 UTC is within lunch hours (11..14)
            var lunchUtc = new DateTime(2025, 10, 20, 12, 0, 0, DateTimeKind.Utc);

            // Act
            var menu = svc.GetMenu(lunchUtc);

            // Assert
            Assert.IsNotNull(menu, "GetMenu should not return null");
            Assert.AreEqual("Lunch Menu", menu.Title, "Menu title should indicate lunch menu");

            // Lunch menu should contain a lunch-specific Margherita priced lower than standard
            var lunchMargherita = menu.Items.FirstOrDefault(i => i.Description.Contains("Lunch Deal: Margherita"));
            Assert.IsNotNull(lunchMargherita, "Lunch Margherita item should exist");
            Assert.AreEqual(6.0, lunchMargherita.Price, 0.0001, "Lunch Margherita should be priced at 6.0");
        }

        [TestMethod]
        public void GetMenu_ReturnsStandardMenuOutsideLunchHours()
        {
            // Arrange
            var svc = GetService();
            // Hour 10 UTC is outside lunch hours -> standard menu
            var morningUtc = new DateTime(2025, 10, 20, 10, 0, 0, DateTimeKind.Utc);

            // Act
            var menu = svc.GetMenu(morningUtc);

            // Assert
            Assert.IsNotNull(menu, "GetMenu should not return null");
            Assert.AreEqual("Standard Menu", menu.Title, "Menu title should indicate standard menu");

            var margherita = menu.Items.FirstOrDefault(i => i.Description == "Margherita");
            Assert.IsNotNull(margherita, "Standard Margherita item should exist");
            Assert.AreEqual(8.0, margherita.Price, 0.0001, "Standard Margherita should be priced at 8.0");
        }
    }
}
