namespace MauiApp20_assignment_5.Views;

public class Profile : ContentPage
{

    public class ProfileModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string Bio { get; set; }

        // Bonus: save the picture path
        public string PhotoPath { get; set; }
    }
}	