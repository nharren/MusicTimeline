namespace NathanHarrenstein.MusicTimeline.Security
{
    public class Credential
    {
        private readonly string applicationName;
        private readonly CredentialType credentialType;
        private readonly string password;
        private readonly string userName;

        public Credential(CredentialType credentialType, string applicationName, string userName, string password)
        {
            this.applicationName = applicationName;
            this.userName = userName;
            this.password = password;
            this.credentialType = credentialType;
        }

        public string ApplicationName
        {
            get
            {
                return applicationName;
            }
        }

        public CredentialType CredentialType
        {
            get
            {
                return credentialType;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
        }

        public string UserName
        {
            get
            {
                return userName;
            }
        }

        public override string ToString()
        {
            return string.Format("CredentialType: {0}, ApplicationName: {1}, UserName: {2}, Password: {3}", CredentialType, ApplicationName, UserName, Password);
        }
    }
}