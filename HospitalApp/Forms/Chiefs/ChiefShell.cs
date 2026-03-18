using HospitalApp.Models;
using HospitalApp.Repositories;

namespace HospitalApp.Forms.Chiefs
{
    // Loads all diet plans linked to the selected appointment into the bottom grid.
    public class ChiefShell: Form
    {
        private User CurrentUser = null!;
        private Chief CurrentChief = null!;

        public ChiefShell(User user)
        {
            CurrentUser = user;

            LoadChief();
            SetupForm();
            SetupLayout();            
        }

        private void SetupForm()
        {
            
        }

        private void SetupLayout()
        {
            
        }

        // Loads the chief profile for the logged-in user; falls back to a default if no profile found.
        private void LoadChief()
        {
            CurrentChief = ChiefRepository.GetByUserId(CurrentUser.UserID)
                ?? new Chief {Fullname = CurrentUser.Username, IsHead = false};
        }
    }
}