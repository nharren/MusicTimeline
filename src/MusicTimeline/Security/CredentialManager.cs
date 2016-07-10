using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace NathanHarrenstein.MusicTimeline.Security
{
    public static class CredentialManager
    {
        private enum CredentialPersistence : uint
        {
            Session = 1,
            LocalMachine,
            Enterprise
        }

        public static IReadOnlyList<Credential> EnumerateCrendentials()
        {
            List<Credential> result = new List<Credential>();

            int count;
            IntPtr pCredentials;
            bool ret = CredEnumerate(null, 0, out count, out pCredentials);

            if (ret)
            {
                for (int n = 0; n < count; n++)
                {
                    IntPtr credential = Marshal.ReadIntPtr(pCredentials, n * Marshal.SizeOf(typeof(IntPtr)));
                    result.Add(ReadCredential((CREDENTIAL)Marshal.PtrToStructure(credential, typeof(CREDENTIAL))));
                }
            }
            else
            {
                int lastError = Marshal.GetLastWin32Error();
                throw new Win32Exception(lastError);
            }

            return result;
        }

        public static Credential ReadCredential(string applicationName)
        {
            IntPtr nCredPtr;

            if (CredRead(applicationName, CredentialType.Generic, 0, out nCredPtr))
            {
                using (CriticalCredentialHandle critCred = new CriticalCredentialHandle(nCredPtr))
                {
                    CREDENTIAL cred = critCred.GetCredential();

                    return ReadCredential(cred);
                }
            }

            return null;
        }

        public static int WriteCredential(string applicationName, string userName, string secret)
        {
            byte[] byteArray = Encoding.Unicode.GetBytes(secret);

            if (byteArray.Length > 512)
            {
                throw new ArgumentOutOfRangeException("secret", "The secret message has exceeded 512 bytes.");
            }

            CREDENTIAL credential = new CREDENTIAL();
            credential.AttributeCount = 0;
            credential.Attributes = IntPtr.Zero;
            credential.Comment = IntPtr.Zero;
            credential.TargetAlias = IntPtr.Zero;
            credential.Type = CredentialType.Generic;
            credential.Persist = (UInt32)CredentialPersistence.Session;
            credential.CredentialBlobSize = (UInt32)Encoding.Unicode.GetBytes(secret).Length;
            credential.TargetName = Marshal.StringToCoTaskMemUni(applicationName);
            credential.CredentialBlob = Marshal.StringToCoTaskMemUni(secret);
            credential.UserName = Marshal.StringToCoTaskMemUni(userName ?? Environment.UserName);

            bool written = CredWrite(ref credential, 0);
            int lastError = Marshal.GetLastWin32Error();

            Marshal.FreeCoTaskMem(credential.TargetName);
            Marshal.FreeCoTaskMem(credential.CredentialBlob);
            Marshal.FreeCoTaskMem(credential.UserName);

            if (written)
                return 0;

            throw new Exception(string.Format("CredWrite failed with the error code {0}.", lastError));
        }

        [DllImport("advapi32", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool CredEnumerate(string filter, int flag, out int count, out IntPtr pCredentials);

        [DllImport("Advapi32.dll", EntryPoint = "CredFree", SetLastError = true)]
        private static extern bool CredFree([In] IntPtr cred);

        [DllImport("Advapi32.dll", EntryPoint = "CredReadW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CredRead(string target, CredentialType type, int reservedFlag, out IntPtr credentialPtr);

        [DllImport("Advapi32.dll", EntryPoint = "CredWriteW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CredWrite([In] ref CREDENTIAL userCredential, [In] UInt32 flags);

        private static Credential ReadCredential(CREDENTIAL credential)
        {
            string applicationName = Marshal.PtrToStringUni(credential.TargetName);
            string userName = Marshal.PtrToStringUni(credential.UserName);
            string secret = null;

            if (credential.CredentialBlob != IntPtr.Zero)
            {
                secret = Marshal.PtrToStringUni(credential.CredentialBlob, (int)credential.CredentialBlobSize / 2);
            }

            return new Credential(credential.Type, applicationName, userName, secret);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CREDENTIAL
        {
            public UInt32 Flags;
            public CredentialType Type;
            public IntPtr TargetName;
            public IntPtr Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public UInt32 CredentialBlobSize;
            public IntPtr CredentialBlob;
            public UInt32 Persist;
            public UInt32 AttributeCount;
            public IntPtr Attributes;
            public IntPtr TargetAlias;
            public IntPtr UserName;
        }

        private sealed class CriticalCredentialHandle : CriticalHandleZeroOrMinusOneIsInvalid
        {
            public CriticalCredentialHandle(IntPtr preexistingHandle)
            {
                SetHandle(preexistingHandle);
            }

            public CREDENTIAL GetCredential()
            {
                if (!IsInvalid)
                {
                    return (CREDENTIAL)Marshal.PtrToStructure(handle, typeof(CREDENTIAL));
                }

                throw new InvalidOperationException("Invalid CriticalHandle!");
            }

            protected override bool ReleaseHandle()
            {
                if (!IsInvalid)
                {
                    CredFree(handle);
                    SetHandleAsInvalid();

                    return true;
                }

                return false;
            }
        }
    }  
}