namespace MauiApp20_assignment_5;

using MauiApp20_assignment_5.Views;
using System.Text.Json;

public partial class PROFILE_PAGE : ContentPage
{
    private readonly string _filePath;
    private string _currentPhotoPath;

    public PROFILE_PAGE()
    {
        InitializeComponent();

        // Path where JSON file will be saved
        _filePath = Path.Combine(FileSystem.AppDataDirectory, "profile.json");

        // Load data when page opens
        LoadProfile();
    }

    private void LoadProfile()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            var profile = JsonSerializer.Deserialize<Profile.ProfileModel>(json);

            if (profile != null)
            {
                NameEntry.Text = profile.Name;
                SurnameEntry.Text = profile.Surname;
                EmailEntry.Text = profile.EmailAddress;
                BioEditor.Text = profile.Bio;

                if (!string.IsNullOrEmpty(profile.PhotoPath) && File.Exists(profile.PhotoPath))
                {
                    ProfileImage.Source = profile.PhotoPath;
                    _currentPhotoPath = profile.PhotoPath;
                }
            }
        }
    }

    private async void OnChoosePhotoClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Select Profile Photo"
            });

            if (result != null)
            {
                string dest = Path.Combine(FileSystem.AppDataDirectory, result.FileName);

                using var srcStream = await result.OpenReadAsync();
                using var destStream = File.OpenWrite(dest);
                await srcStream.CopyToAsync(destStream);

                ProfileImage.Source = dest;
                _currentPhotoPath = dest;
            }
        }
        catch
        {
            // Ignore errors
        }
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {
        var profile = new Profile.ProfileModel
        {
            Name = NameEntry.Text,
            Surname = SurnameEntry.Text,
            EmailAddress = EmailEntry.Text,
            Bio = BioEditor.Text,
            PhotoPath = _currentPhotoPath
        };

        string json = JsonSerializer.Serialize(profile);

        File.WriteAllText(_filePath, json);

        DisplayAlert("Success", "Profile saved successfully!", "OK");
    }
}