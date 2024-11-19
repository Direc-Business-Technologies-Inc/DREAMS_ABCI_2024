using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ABROWN_DREAMS
{
    public class LicenseHelper
    {
        private static AssemblyInfo _info = new AssemblyInfo(Assembly.GetEntryAssembly());

        public static void CreatePreRegistrationID(string filePath,
                                                    string computerName,
                                                    string hardwareKey,
                                                    string companyName,
                                                    string contactPerson,
                                                    string email,
                                                    string ePassword)
        {

            string SerialScript = null;
            DateTime dt = DateTime.Now;
            string oDateTime = Convert.ToString(DateTime.Now);
            string RegistrationID = dt.ToString("MMddyyyyHHmmss");

            //this code segment write data to file.
            using (TextWriter tw = new StreamWriter(filePath))
            {
                tw.WriteLine($"Registration File Created From { companyName } At { oDateTime } With Control Number : { RegistrationID }");
                tw.WriteLine($"PRODUCT NAME : { _info.Product }");
                tw.WriteLine($"PRODUCT VERSION : { _info.FileVersion }");
                tw.WriteLine($"COMPUTER NAME : { computerName }");
                tw.WriteLine($"COMPANY NAME : { companyName }");
                tw.WriteLine($"CONTACT PERSON : { contactPerson }");
                tw.WriteLine($"EMAIL ADDRESS : { email }");

                SerialScript = "<?/=wEPDwUKLTkzNzUxOTU4MQ9kFgJmD2QWAgIBD2QWCgIBDxQrAAYPFgIe" +
                "Dl8hVXNlVmlld1N0YXRlZ2Q8KwAOAQAWAh4HT3BhY2l0eQJGZGRkDxQrAAYUKwAGFgoeBFRleHQFB" +
                "0NvbXBhbnkeBE5hbWUFB0NvbXBhbnkeC05hdmlnYXRlVXJsBRtWaWV3Q29udGVudC5hc3B4P3JlZ2lvbklEPT" +
                "MeBlRhcmdldGUeDlJ1bnRpbWVDcmVhdGVkZxAWCGYCAQICAgMCBAIFAgYCBxYIPCsABgEAFgofAgUKQWJvdXQg" +
                "WENFTB8DBQpBYm91dCBYQ0VMHwQFG1ZpZXdDb250ZW50LmFzcHg/cmVnaW9uSUQ9OR8FZR8GZzwrAAYBA" +
                "BYKHwIFFEJ1c2luZXNzIFByb3Bvc2l0aW9uHwMFFEJ1c2luZXNzIFByb3Bvc2l0aW9uHwQFHFZpZXdDb250ZW50L";

                SerialScript = SerialScript + "mFzcHg/cmVnaW9uSUQ9MTAfBWUfBmc8KwAGAQAWCh8CBQxBZmZpbGlhdGlvbnMfAwUMQWZmaWxpYXRpb25zHwQFHF" +
                "ZpZXdDb250ZW50LmFzcHg/cmVnaW9uSUQ9MTEfBWUfBmc8KwAGAQAWCh8CBQ5DZXJ0aWZpY2F0aW9ucx8DBQ5DZXJ0" +
                "aWZpY2F0aW9ucx8EBRxWaWV3Q29udGVudC5hc3B4P3JlZ2lvbklEPTEyHwVlHwZnPCsABgEAFgofAgUSQ29tbXVuaXR5IFNlcnZ" +
                "pY2VzHwMFEkNvbW11bml0eSBTZXJ2aWNlcx8EBRxWaWV3Q29udGVudC5hc3B4P3JlZ2lvbklEPTEzHwVlHwZnPCsABgEAFgofAgUR" +
                "RGl2ZXJzaXR5IFByb2dyYW0fAwURRGl2ZXJzaX" + $"3Qg1{ dt.ToString("MM") }3Qg2" + "R5IFByb2dyYW0fBAUcVmlld0NvbnRlbnQuYXNweD9yZWdpb25JRD0xNB8FZR" +
                "8GZzwrAAYBABYKHwIFC0dPIEdSRUVOIDopHwMFC0dPIEdSRUVOIDopHwQFHFZpZXdDb250ZW50LmFzcHg/cmVnaW9uSUQ9MTUfBWUf" +
                "Bmc8KwAGAQAWCh8CBQlMb2NhdGlvbnMfAwUJTG9jYXRpb25zHwQFHFZpZXdDb250ZW50LmFzcHg/cmVnaW9uSUQ9MTYfBWUfBmdkZGRkZBQ" +
                "rAAYWCh8CBQlTb2x1dGlvbnMfAwUJU29sdXRpb25zHwQFG1ZpZXdDb250ZW50LmFzcHg/cmVnaW9uSUQ9NB8FZR8GZxAWAmYCARYCPCsABg" +
                "EAFgofAgUUVGVjaG5vbG9neSBTb2x1dGlvbnMfAwUUVGVjaG5vbG9neSBTb2x1dGlvbnMfBAUcVmlld0NvbnRlbnQuYXNweD9yZWdpb25JRD0xNx8F";

                SerialScript = SerialScript + "ZR8GZzwrAAYBABYKHwIFEkluZHVzdHJ5IFNvbHV0aW9ucx8DBRJJbmR1c3RyeSBTb2x1dGlvbnMfBAUcVmlld0NvbnRlbnQuYXNweD9yZWdpb25J" +
                "RD0xOB8FZR8GZ2RkZGRkFCsABhYKHwIFCFNlcnZpY2VzHwMFCFNlcnZpY2VzHwQFG1ZpZXdDb250ZW50LmFzcHg/cmVnaW9uSUQ9NR8FZR8GZ" +
                "xAWCGYCAQICAgMCBAIFAgYCBxYIPCsABgEAFgofAgUUU29mdHdhcmUgRGV2ZWxvcG1lbnQfAwUUU29mdHdhcmUgRGV2ZWxvcG1lbnQfBA" +
                "UcVmlld0NvbnRlbnQuYXNweD9yZWdpb25JRD0xOR8FZR8GZzwrAAYBABYKHwIFFVByb2Zlc3Npb25hbCBTZXJ2aWNlcx8DBRVQcm9mZXNzaW" +
                "9uYWwgU2VydmljZXMfBAUcVmlld0NvbnRlbnQuYXNweD9yZWdpb25JRD0yMB8FZR8GZzwrAAYBABYKHwIFElByb2plY3dQgTWFuYWdlbWVudB8DB" +
                "RJQcm9qZWN0IE1hbmFnZW1lbnQfBAUcVmlld0NvbnRlbn" + $"3Qg1{ dt.ToString("dd") }3Qg2" + "QuYXNweD9yZWdpb25JRD0yMR8FZR8GZzwrAAYBABYKHwIFDm5TaG9yZSBTZXJ" +
                "2aWNlHwMFDm5TaG9yZSBTZXJ2aWNlHwQFHFZpZXdDb250ZW50LmFzcHg/cmVnaW9uSUQ9MjIfBWUfBmc8KwAGAQAWCh8CBSR" +
                "JbmZyYXN0cnVjdHVyZSAmIFBlcmZvcm1hbmNlIEFuYWx5aXMfAwUkSW5mcmFzdHJ1Y3R1cmUgJiBQZ?/>Jmb3JtYW5jZSBBbmFseWlzHwQFH" +
                "FZpZXdDb250ZW50LmFzcHg/cmVnaW9uSUQ9MjMfBWUfBmc8KwAGAQAWCh8CBRVSZXNlYWNoICYgRGV2ZWxvcG1lbnQfAwUVUmVzZ" +
                "WFjaCAmIERldmVsb3BtZW50HwQFHFZpZXdDb250ZW50LmFzcHg/cmVnaW9uSUQ9MjQfBWUfBmc8KwAGAQAWCh8CBRhUcmFpbmluZyBhb";

                SerialScript = SerialScript + "mQgRGV2ZWxvcG1lbnQfAwUYVHJhaW5pbmcgYW5kIERldmVsb3BtZW50HwQFHFZpZXdDb250ZW50LmFzcHg/cmVnaW9uSUQ9MjUfBWUfB" +
                "mc8KwAGAQAWCh8CBQ1XZSBJbnZlc3dQgISEhHwMFDVdlIEludmVzdCAhISEfBAUcVmlld0NvbnRlbnQuYXNweD9yZWdpb25JRD0yNh8FZR8GZ2" +
                "RkZGRkPCsABgEAFgofAgUJQ3VzdG9tZXJzHwMFCUN1c3RvbWVycx8EBR1WaWV3Q3VzdG9tZXJzLmFzcHg/cmVnaW9uSUQ9Nh8FZR8GZxQrA" +
                "AYWCh8CBQhQYXJ0bmVycx8DBQhQYXJ0bmVycx8EBRtWaWV3Q29udGVudC5hc3B4P3JlZ2lvbklEPTcfBWUfBmcQFgNmAgECAhYDPCsABgEAFgo" +
                "fAgUMT3VyIFBhcnRuZXJzHwMFDE91ciBQYXJ0bmVycx8EBR" + $"3Qg1{ dt.ToString("yyyy") }3Qg2" + "xWaWV3Q29udGVudC5hc3B4P3JlZ2lvbklEPTI3HwVlHwZnPCsABgEAFgofAgUPU" +
                "GFydG5lciBQcm9ncmFtHwMFD1BhcnRuZXIgUHJvZ3JhbR8EBRxWaWV3Q29udGVudC5hc3B4P3JlZ2lvbklEPTI4HwVlHwZnPCsABgEAFgofAg" +
                "UMSm9pbiBPdXIgUiZEHwMFDEpvaW4gT3VyIFImRB8EBRxWaWV3Q29udGVudC5hc3B4P3JlZ2lvbklEPTI5HwVlHwZnZGRkZGQUKwAGFgofAgUET" +
                "mV3cx8DBQROZXdzHwQFG05ld3NMZXR0ZXJzLmFzcHg/cmVnaW9uSUQ9OB8FZR8GZxAWBGYCAQICAgMWBBQrAAYWCh8CBQZBd2FyZHM" +
                "fAwUGQXdhcmRzHwQFHFZpZXdDb250ZW50LmFzcHg/cmVnaW9uSUQ9MzAfB<?/=WUfBmcQFghmAgECAgIDAgQCBQIGAgcWCDwrAAYB";

                SerialScript = SerialScript + "ABYKHwIFCEJQVyAyMDA4HwMFCEJQVyAyMDA4HwQFHFZpZXdDb250ZW50LmFzcHg/cmVnaW9uSUQ9MzMfBWUfBmc8KwAGAQAWCh8CBRJEa" +
                "XZlcnNpdHkgQnVzaW5lc3MfAwUSRGl2ZXJzaXR5IEJ1c2luZXNzHwQFHFZpZXdDb250ZW50LmFzcHg/cmVnaW9uSUQ9MzQfBWUfBmc8KwAGA" +
                "QAWCh8CBRNEZWxvaXR0ZSAtIEZhc3dQgNTAwHwMFE0RlbG9pdHRlIC0gRmFzdCA1MDAfBAUcVmlld0NvbnRlbnQuYXNweD9yZWdpb25JRD0zNR" +
                "8FZR8GZzwrAAYBABYKHwIFDE5KIEZpbmVzdCA1MB8DBQxOSiBGaW5lcs3QfgNTAfBAUcVmlld0NvbnRlbnQuYXNweD9yZWdpb25JRD0zNh8FZR8" +
                "GZzwrAAYBABYKHwIFEkRlbG9pdHRlIC0gRmFzdCA1MB8DBRJEZWxvaXR0ZSAtIEZhc3QfgNTAfBAUcVmlld0NvbnRlbnQuYXNweD9yZWdpb25JRD0zNx" +
                "8FZR8GZzwrAAYBABYKHwIFDUZvcnR5IFVuZXIgNDAfAwUNRm9ydHkgVW5lciA0MB8EBRxWaWV3Q29udGVudC5hc3B4P3JlZ2lvbklEPTM4" +
                "HwVlHwZnPCsABgEAFgofAgUZU0FQIEJlc3QfgU2FsZXMgRXhjZWxsZW5jZR8DBRlTQVAgQmVzdCBTYWxlcyBFeGNlbGxlbmNlHwQFHFZ" +
                "pZXdDb250ZW50LmFzcHg/cmVnaW9uSUQ9MzkfBWUfBmc8KwAGAQAWCh8CBQdCUFcyMDA3HwMFB0JQVzIwMDcfBAUcVmlld0NvbnRlbnQuY" +
                "XNweD9yZWdpb25JRD00MB8FZR8GZ2RkZGRkPCsABgEAFgofAgUOTWVkaWEgQ292ZXJhZ2UfAwUOTWVkaWEgQ292ZXJhZ2UfBAUcVmll" +
                "d0NvbnRlbnQuYXNweD9yZWdpb25JRD0zMR8FZR8GZzwrAAYBABYKHwIFBkV2ZW50cx8DBQZFdmVudHMfBAUcVmlld0NvbnRlbnQuYXNw";

                SerialScript = SerialScript + "eD9yZWdpb25JRD0zMh8FZR8GZzwrAAYBABYKHwIFC05ld3NsZXR0ZXJzHwMFC05ld3NsZXR0ZXJzHwQFHE5ld3NMZXR0ZXJzLmFzcHg/" +
                "cmVnaW9uSUQ9NTIfBWUfBmdkZGRkZGRkAgoPZBYEZg8WAh4LXyFJdGVtQ291bnQCARYCZg9kFgICAQ9kFgRmD2QWAmYPFQG4AzxwIGFsa" +
                "WduPSJjZW50ZXIiPjxzdHJvbmcPGZvbnQgY29sb3I9IiNmZmZmZmYiIHNpemU9IjMiPldlbGNvbWUgdG8gWENFTCBVU0EhPC9mb250Pjwvc3R" +
                "yb25nPjxmb250IHNpemU9IjUiPiA8L2ZvbnQPHNwYW4gY2xhc3M9ImhlYWQxIHN0eWxlMiIPHNwYW4gY2xhc3M9InN0eWxlNSIPHN0cm9u" +
                "Zz48Zm9udCBjb2xvcj0iIzgwMDAwMCIgc2l6ZT0iNSIWENFTDwvZm9udD48L3N0cm9uZz48ZW0+bGVuY2U8L2VtPjwvc3Bhbj48L3NwYW4+P" +
                "GJyIC8+DQo8c3BhbiBjbGFzcz0iaGVhZDEgc3R5bGUyIj5pbiBxdWFsaXR5IHNlcnZpY2VzLDwvc3Bhbj48YnIgLz4NCjxzcGFuIGNsYXNzPSJoZW" +
                "FkMSBzdHlsZTIiPmJ1c2luZXNzIHZhbHVlcyw8L3NwYW4+PGJyIC8+DQo8c3BhbiBjbGFzcz0iaGVhZDEgc3R5bGUyIj5hbmQgZXRoaWNhbCBwcmlu" +
                "Y2lwbGVzLjwvc3Bhbj48L3AZAIBD2QWAgIBDxYCHgpvbmRibGNsaWNrBXFzaG93UG9wV2luKCdhZG1pbi9zZWN0aW9uX2VkaXRvci5hc3B4P2d1aW" +
                "Q9M2MwZjAzMTctODkwYy00MjhiLWJhYjYtNTRkYTQ5NjdkMzI1JnNuPUhvbWVfQmFubmVyJicsIDczMCwgNjAwLCBudWxsKRYCZg8VAbgDPHAgY" +
                "WxpZ249ImNlbnRlciIPHN0cm9uZz48Zm9udCBjb2xvcj0iI2ZmZmZmZiIgc2l6ZT0iMyIV2VsY29tZSB0byBYQ0VMIFVTQSE8L2ZvbnQPC9zdHJvbmcPG";

                SerialScript = SerialScript + "ZvbnQgc2l6ZT0iNSIIDwvZm9udD48c3BhbiBjbGFzcz0iaGVhZDEgc3R5bGUyIj48c3BhbiBjbGFzcz0ic3R5bGU1Ij48c3Ryb25nPjxmb250IGNvbG9yP" +
                "SIjODAwMDAwIiBzaXplPSI1Ij5YQ0VMPC9mb250Pjwvc3Ryb25nPjxlbT5sZW5jZTwvZW0+PC9zcGFuPjwvc3Bhbj48YnIgLz4NCjxzcGFuIGNsYXNzPSJo" +
                "ZWFkMSBzdHlsZTIiPmluIHF1YWxpdHkgc2VydmljZXMsPC9zcGFuPjxiciAvPg0KPHNwYW4gY2xhc3M9ImhlYWQxIHN0eWxlMiIYnVzaW5lc3MgdmFsdW" +
                "VzLDwvc3Bhbj48YnIgLz4NCjxzcGFuIGNsYXNzPSJoZWFkMSBzdHlsZTIiPmFuZCBldGhpY2FsIHByaW5jaXBsZXMuPC9zcGFuPjwvcD5kAgIPDxYCHgdWa" +
                "XNpYmxlaGRkAgsPZBYQAgEPZBYEZg8WAh8HAgIWBGYPZBYCAgEPZBYEZg9kFgJmDxUBtgc8cCBhbGlnbj0iY2VudGVyIj48Zm9udCBzaXplPSI0Ij48c3Ry" +
                "b25nPjwvc3Ryb25nPjwvZm9udD48c3BhbiBjbGFzcz0iYm9keWJvbGRnb2xnIiBfX2Rlc2lnbmVyOmR0aWQ9IjM5NDA3MzEyNzgzMjc4MzkiPjxmb250IHNpe" +
                "mU9IjQiPjxzcGFuIGNsYXNzPSJib2R5Ym9sZGdvbGciIF9fZGVzaWduZXI6ZHRpZD0iMzk0MDczMTI3ODMyNzgzOSIPGZvbnQgY29sb3I9IiNjYjk4MzIiI" +
                "HNpemU9IjQiPldlbGNvbWUgdG8mbmJzcDsgWENFTCAtIDxlbT5MZWFkaW5nIHRoZSBGdXR1cmUuLi4gPC9lbT48L2ZvbnQPC9zcGFuPjwvZm9udD48L3" +
                "NwYW4+PC9wPg0KPHAgYWxpZ249Imp1c3RpZnkiPjxzdHJvbmcPGVtPjxmb250IGNvbG9yPSIjODAwMDAwIj5YQ0VMIFNvbHV0aW9ucyBDb3JwLjwvZm9u" +
                "dD48L2VtPjwvc3Ryb25nPiBpcyBhIHByZW1pZXIgSW52678B249EF261D8259CCmb3JtYXRpb24gVGVjaG5vbG9neSBzZXJ2aWNlIHByb3ZpZGVyIHdpdGgm";

                SerialScript = SerialScript + "bmJzcDthIHByb3ZlbiZuYnNwO3RyYWNrIHJlY29yZCBvZiBwcm92aWRpbmcgdmFsdWUtYWRkZWQgc2VydmljZXMgdG8gRm9ydHVuZS01MDAgY29tcGFuaWVz" +
                "LiBPdXIgZm9jdXMgaGFzIGFsd2F5cyBiZWVuIGluIGJ1aWxkaW5nIHN0cm9uZyByZWxhdGlvbnNoaXBzIHRocm91Z2ggb3VyIHNlcnZpY2UgZXhj" +
                "ZWxsZW5jZSBhbmQgY29zdCBjb250YWlubWVudCBwcmFjdGljZXMgZm9yIG91ciBjdXN0b21lcnMuPC9wPg0KPHAgY2xhc3M9ImJvZHlUZXh0IiBhbGlnb" +
                "j0ianVzdGlmeSIV2UgaGF2ZSAzIG1ham9yIGdsb2JhbCBvcGVyYXRpb25zIGNlbnRlcnMgPGZvbnQgY29sb3I9IiM4MDAwMDAiPk5ldyBKZXJzZXksIFVTQTs" +
                "gTWV4aWNvIENpdHksIE1leGljbzsgTWFuaWxhLCBQaGlsaXBwaW5lczs8L2ZvbnQIGNhdGVyaW5nIG91ciBjbGllbnRzIGF0IHZhcmlvdXMgZ2VvZ3JhcGhp" +
                "Y2FsIGxvY2F0aW9ucyB3b3JsZHdpZGUuIDxiciAvPg0KPC9wPmQCAQ9kFgICAQ8WAh8IBXJzaG93UG9wV2luKCdhZG1pbi9zZWN0aW9uX2VkaXRvci5h" +
                "c3B4P2d1aWQ9NGFjY2NlNGItNjgxMy00ZWUxLWFlYjMtYmFjOTU1MjkzYzc2JnNuPUhvbWVfV2VsY29tZSYnLCA3MzAsIDYwMCwgbnVsbCkWAmYPFQG" +
                "2BzxwIGFsaWduPwWw2678B249EF261D8259CCSJwwdasda.jZW50ZXIiPjxmb250IHNpemU9IjQiPjxzdHJvbmcPC9zdHJvbmcPC9mb250PjxzcGFuIGNsY" +
                "XNzPSJib2R5Ym9sZGdvbGciIF9fZGVzaWduZXI6ZHRpZD0iMzk0MDczMTI3ODMyNzgzOSIPGZvbnQgc2l6ZT0iNCIPHNwYW4gY2xhc3M9ImJvZHlib2" +
                "xkZ29sZyIgX19kZXNpZ25lcjpkdGlkPSIzOTQwNzMxMjc4MzI3ODM5Ij48Zm9udCBjb2xvcj0iI2NiOTgzMiIgc2l6ZT0iNCIV2VsY29tZSB0byZuYnNwOyBYQ0VM";

                SerialScript = SerialScript + "IC0gPGVtPkxlYWRpbmcgdGhlIEZ1dHVyZS4uLiA8L2VtPjwvZm9udD48L3NwYW4+PC9mb250Pjwvc3Bhbj48L3ADQo8cCBhbGlnbj0ianVzdGlmeSIPHN0cm9uZz" +
                "48ZW0+PGZvbnQgY29sb3I9IiM4MDAwMDAiPlhDRUwgU29sdXRpb25zIENvcnAuPC9mb250PjwvZW0+PC9zdHJvbmcIGlzIGEgcHJlbWllciBJbmZvcm1hdGl" +
                "vbiBUZWNobm9sb2d5IHNlcnZpY2UgcHJvdmlkZXIgd2l0aCZuYnNwO2EgcHJvdmVuJm5ic3A7dHJhY2sgcmVjb3JkIG9mIHByb3ZpZGluZyB2YWx1ZS1hZGRl" +
                "ZCBzZXJ2aWNlcyB0byBGb3J0dW5lLTUwMCBjb21wYW5pZXMuIE91ciBmb2N1cyBoYXMgYWx3YXlzIGJlZW4gaW4gYnVpbGRpbmcgc3Ryb25nIHJlbGF0aW9" +
                "uc2hpcHMgdGhyb3VnaCBvdXIgc2VydmljZSBleGNlbGxlbmNlIGFuZCBjb3N0IGNvbnRhaW5tZW50IHByYWN0aWNlcyBmb3Igb3VyIGN1c3RvbWVycy48L3ADQ" +
                "o8cCBjbGFzcz0iYm9keVRleHQiIGFsaWduPSJqdXN0aWZ5Ij5XZSBoYXZlIDMgbWFqb3IgZ2xvYmFsIG9wZXJhdGlvbnMgY2VudGVycyA8Zm9udCBjb2xvcj0iIz" +
                "gwMDAwMCITmV3IEplcnNleSwgVVNBOyBNZXhpY28gQ2l0eSwgTWV4aWNvOyBNYW5pbGEsIFBoaWxpcHBpbmVzOzwvZm9udD4gY2F0ZXJpbmcgb3VyIGNs" +
                "aWVudHMgYXQgdmFyaW91cyBnZW9ncmFwaGljYWwgbG9jYXRpb25zIHdvcmxkd2lkZS4gPGJyIC8+DQo8L3AZAIBD2QWAgIBD2QWBGYPZBYCZg8VAQBkAg" +
                "EPZBYCAgEPFgIfCAVyc2hvd1BvcFdpbignYWRtaW4vc2VjdGlvbl9lZGl0b3IuYXNweD9ndWlkPWIzMjAyZTk3LWNjMjgtNDAzZC1iM2EyLTIzYWQ0NWE3MDQz" +
                "OSZzbj1Ib21lX1dlbGNvbWUmJywgNzMwLCA2MDAsIG51bGwpFgJmDxUBAGQCAg8PFgIfCWhkZAIDD2QWBGYPFgIfBwIBFgJmD2QWAgIBD2QWBGYPZBYC" +
                "Zg8VARRUZWNobm9sb2d5IEV4cGVydGlzZWQCAQ9kFgICAQ8WAh8IBXhzaG93UG9wV2luKCdhZG1pbi9zZWN0aW9uX2VkaXRvci5" + $"3Qg1{ DataCipher.DataEncrypt(computerName.Trim()) }3Qg2" + "hc3B4P2d1aWQ9ZG";

                SerialScript = SerialScript + "FjOTI3Y2EtMTQ2NC00Nzk5LTk5MzMtMzFiODg1YjMyOGQ0JnNuPUhvbWVfQ29sdW1uX1RpdGxlMSYnLCA3MzAsIDYwMCwgbnVsbCkWAmYPFQEUVGVja" +
                "G5vbG9neSBFeHBlcnRpc2VkAgIPDxYCHwloZGQCBQ9kFgRmDxYCHwcCARYCZg9kFgICAQ9kFgRmD2QWAmYPFQHqATxwIGFsaWduPSJqdXN0aWZ5IiBjbGFz" +
                "cz0iYm9keVRleHQiPlhDRUwgcHJvdmlkZXMgYXBwbGljYXRpb24gYW5kIGJ1c2luZXNzIGV4cGVydGlzZSB0byBjbGllbnRzIGluIGFsbCBzZWdtZW50cyBvZiBpbmR" +
                "1c3RyaWVzLiBUaGUgY29yZSBlbGVtZW50cyBpbmNsdWRlIGFwcGxpY2F0aW9uIHNvbHV0aW9ucyB3aXRoaW4gRVJQLCBDUk0sIFNFTSwgU0NNIHdpdGggU0FQ" +
                "LCBPcmFjbGUsIFBlb3BsZVNvZnQuPC9wPmQCAQ9kFgICAQ8WAh8IBXJzaG9UG9wV2luKCdhZG1pbi9zZWN0aW9uX2VkaXRvci5hc3B4P2d1aWQ9OGFkZDIzOD" +
                "ctYmM2Yi00NjBiLWFhNDktODA2NGI0ZmIwOTg2JnNuPUhvbWVfQ29sdW1uMSYnLCA3M<?/=zAsIDYwMCwgbnVsbCkWAmYPFQHqATxwIGFsaWduPSJqdXN0aW" +
                "Z5IiBjbGFzcz0iYm9keVRleHQiPlhDRUwgcHJvdmlkZXMgYXBwbGljYXRpb24gYW5kIGJ1c2luZXNzIGV4cGVydGlzZSB0byBjbGllbnRzIGluIGFsbCBzZWdtZW5" +
                "0cyBvZiBpbmR1c3RyaWVzLiBUaGUgY29yZSBlbGVtZW50cyBpbmNsdWRlIGFwcGxpY2F0aW9uIHNvbHV0aW9ucyB3aXRoaW4gRVJQLCBDUk0sIFNFTSwgU" +
                "0NNIHdpdGggU0FQLCBPcmFjbGUsIFBlb3BsZVNvZnQuPC9wPmQCAg8PFgIfCWhkZAIHD2QWBGYPFgIfBwICFgRmD2QWAgIBD2QWBGYPZBYCZg8VARBPdXIg" +
                "U2VydmljZXM8JOEYBARYANDANTEYnIZAIBD2QWAgIBDxYCHwgFeHNob3dQb3BXaW4oJ2FkbWluL3NlY3Rpb25fZWRpdG9yLmFzcHg/Z3VpZD1mMzc5Y";

                SerialScript = SerialScript + "WVlYS0yOWQ2LTQ0OTEtODU3Yi04ZDllMjllMDQ5OTcmc249SG9tZV9Db2x1bW5fVGl0bGUyJicsIDczMCwgNjAwLCBudWxsKRYCZg8VARBPdXIgU2VydmljZ" +
                "XM8YnIZAIBD2QWAgIBD2QWBGYPZBYCZg8VAQBkAgEPZBYCAgEPFgIfCAV4c2hvd1BvcFdpbignYWRtaW4vc2VjdGlvbl9lZGl0b3IuYXNweD9ndWlkPTJiOGF" +
                "kNWE5LWNmZTEtNDYwYS05ZTZiLWY5ZGY3NjUwNGZkNCZzbj1Ib21lX0NvbHVtbl9UaXRsZTImJywgNzMwLCA2MDAsIG51bGwpFgJmDxUBAGQCAg8PFgIfC" +
                "WhkZAIJD2QWBGYPFgIfBwIBFgJmD2QWAgIBD2QWBGYPZBYCZg8VAckDPHAgYWxpZ249Imp1c3RpZnkiIGNsYXNzPSJib2R5VGV4dCIWENFTCBTb2x1dGl" +
                "vbnMgQ29ycG9yYXRpb24gaXMgYSBsZWFkZXIgaW4gU29mdHdhcmUgQ29uc3VsdGluZywgUHJvamVjdCBNYW5hZ2VtZW50LCBJVCBTdGFmZmluZyBTb2x1dGlv" +
                "bnMsIE91dHNvdXJjaW5nIChCUE8pLCBDdXN0b20gU29sdXRpb25zIGFuZCBFbnRlcnByaXNlIFNvbHV0aW9ucy4gPC9wPg0KPHAgYWxpZ249Imp1c3RpZnkiPldlIH" +
                "NwZWNpYWxpemUgZW5kLTItZW5kIHNvbHV0aW9ucyBmb3IgYWxsIHZlcnRpY2FsIGluZHVzdHJ5IHNvbHV0aW9ucyBub3dQgbGltaXRlZCB0byBFUlAvQ1JNICh" +
                "TQVAsIE9yYWNsZSwgUGVvcGxlU29mdCwgU2llYmVsIGV0Yy4sKSwgQnVzaW5lc3MgSW50ZWxsaWdlbmNlLCBEYXRhd2FyZSBIb3VzZSwgSmF2YSBUZWNobm" +
                "9sb2dpZXMsIE1pY3Jvc29mdCBzb2x1dGlvbnMgZXRjLjxiciAvPg0KPC9wPmQCAQ9kFgICAQ8WAh8IBXJzaG93UG9wV2luKCdhZG1pbi9zZWN0aW9uX2VkaXR" +
                "vci5hc3B4P2d1aWQ9NzZjYjM5NWUtZmYyYy00MWYxLWE5YmEtZGZmZjg2YmNkZTFiJnNuPUhvbWVfQ29sdW1uMiYnLCA3MzAsIDYwMCwgbnVsbCkWAm";

                SerialScript = SerialScript + "YPFQHJAzxwIGFsaWduPSJqdXN0aWZ5IiBjbGFzcz0iYm9keVRleHQiPlhDRUwgU29sdXRpb25zIENvcnBvcmF0aW9uIGlzIGEgbGVhZGVyIGluIFNvZnR3YXJl" +
                "IENvbnN1bHRpbmcsIFByb2plY3dQgTWFuYWdlbWVudCwgSVQgU3RhZmZpbmcgU29sdXRpb25zLCBPdXRzb3VyY2luZyAoQlBPKSwgQ3VzdG9tIFNvbHV0aW9ucy" +
                "BhbmQgRW50ZXJwcmlzZSBTb2x1dGlvbnMuIDwvcD4NCjxwIGFsaWduPSJqdXN0aWZ5Ij5XZSBzcGVjaWFsaXplIGVuZC0yLWVuZCBzb2x1dGlvbnMgZm9yIGF" +
                "sbCB2ZXJ0aWNhbCBpbmR1c3RyeSBzb2x1dGlvbnMgbm90IGxpbWl0ZWQgdG8gRVJQL0NSTSAoU0FQLCBPcmFjbGUsIFBlb3BsZVNvZnQsIFNpZWJlbCBldG" +
                "MuLCksIEJ1c2luZXNzIEludGVsbGlnZW5jZSwgRGF0YXdhcmUgSG91c2UsIEphdmEgVGVjaG5vbG9naWVzLCBNaWNyb3NvZnQgc29sdXRpb25zIGV0Yy48YnIgLz" +
                "4NCjwvcD5kAgIPDxYCHwloZGQCCw9kFgRmDxYCHwcCARYCZg9kFgICAQ9kFgRmD2QWAmYPFQELT3VyIENsaWVudHNkAgEPZBYCAgEPFgIfCAV4c2hvd1BvcF" +
                "dpbignYWRtaW4vc2VjdGlvbl9lZGl0b3IuYXNweD9ndWlkPWNhNjg0OGUwLWRmNGMtNDVkZS05NjAwLTZiN2ZiZTIwYmI1OSZzbj1Ib21lX0NvbHVtbl9UaXRsZ" +
                "TMmJywgNzMwLCA2MDAsIG51bGwpFgJmDxUBC091ciBDbGllbnRzZAICDw8WAh8JaGRkAg0PZBYEZg8WAh8HAgEWAmYPZBYCAgEPZBYEZg9kFgJmDxUBjgI" +
                "8cCBhbGlnbj0ianVzdGlmeSIgY2xhc3M9ImJvZHlUZXh0Ij5PdXIgbW9zdCB2YWx1YWJsZSBhc3NldHMgYXJlIG91ciBlc3RlZW1lZCBjdXN0b21lcnMgYW5kIGhpZ" +
                "2hseSBza2lsbGVkIGFuZCB0YWxlbnRlZCBlb<?/=XBsb3llZXMuIFhDRUwgcHJvbWlzZXMgdG8gcHJvdGVjdCBpdHMgYXNzZXRzIHRvIGl0cyBmdWxsIGV4dGVudC";

                SerialScript = SerialScript + "4mbmJzcDsgT3VyIHNlcnZpY2UgaW5jbHVkZXMgYWxsIHNpemUgY29tcGFuaWVzIGluIGFsbCB2ZXJ0aWNhbCBtYXJrZXQgc2VnbWVudHMuIDwvcD5kAgEPZB" +
                "YCAgEPFgIfCAVyc2hvd1BvcFdpbignYWRtaW4vc2VjdGlvbl9lZGl0b3IuYXNweD9ndWlkPWUzYTA0YTI5LWY4ZGEtNGQ0Zi04ODhjLTIyMTIyY2M1YjI4NCZzbj1" +
                "Ib21lX0NvbHVtbjMmJywgNzMwLCA2MDAsIG51bGwpFgJmDxUBjgI8cCBhbGlnbj0ianVzdGlmeSIgY2xhc3M9ImJvZHlUZXh0Ij5PdXIgbW9zdCB2YWx1YWJsZS" +
                "Bhc3NldHMgYXJlIG91ciBlc3RlZW1lZCBjdXN0b21lcnMgYW5kIGhpZ2hseSBza2lsbGVkIGFuZCB0YWxlbnRlZCBlbXBsb3llZXMuIFhDRUwgcHJvbWlzZXMgdG8gc" +
                "HJvdGVjdCBpdHMgYXNzZXRzIHRvIGl0cyBmdWxsIGV4dGVudC4mbmJzcDsgT3VyIHNlcnZpY2UgaW5jbHVkZXMgYWxsIHNpemUgY29tcGFuaWVzIGluIGFsbCB" +
                "2ZXJ0aWNhbCBtYXJrZXQgc2VnbWVudHMuIDwvcD5kAgIPDxYCHwloZGQCDw9kFgRmDxYCHwcCARYCZg9kFgICAQ9kFgRmD2QWAmYPFQHrCjxwIGFsaWdu" +
                "PSJqdXN0aWZ5Ij48c3Ryb25nPjx1Pjxmb250IGNvbG9yPSIjODAwMDAwIj5ORVdTIEZMQVNIIDxiciAvPg0KPGJyIC8+DQo8L2ZvbnQPGZvbnQgY29sb3I9IiM" +
                "wMDAwZmYiIHNpemU9IjIiPioqKiBXZSBhcmUgY3VycmVudGx5IGVuaGFjaW5nIG91ciB3ZWJzaXRlLiBJdCB3aWxsIGdvIGxpdmUgb24gSnVseSAwMSwgMjAwO" +
                "S4gUGxlYXNlIHJldmlzaXQgdXMgZm9yIG1vcmUgaW5mb3JtYXRpb24uIFNvcnJ5IGZvciB0aGUgaW5jb3ZlbmllbmNlLjwvZm9udD48L3UPC9zdHJvbmc9wPg0KPG" +
                "hyIHNpemU9IjkiIHN0eWxlPSJ3aWR0aDogNDgwcHg7IGhlaWdodDogOXB4OyIgLz4NCjx0YWJsZSBjZWxsc3BhY2luZz0iMSIgY2VsbHBhZGRpbmc9IjEiIGJvc";

                SerialScript = SerialScript + "mRlcj0iMCIgd2lkdGg9IjEwMCUiIHN1bW1hcnk9IiIDQogICAgPHRib2R5Pg0KICAgICAgICA8dHIDQogICAgICAgICAgICA8dGQDQogICAgICAgICAgICA8cCBhb" +
                "Glnbj0ianVzdGlmeSIV2VsY29tZSB0byA8c3Ryb25nPjxmb250IGNvbG9yPSIjODAwMDAwIj5TQU1LVSBSZXNlYXJjaCBMYWI8L2ZvbnQLjwvc3Ryb25nPiBYQ0V" +
                "MIENvcnAuIGhhcyBiZWVuIGRlZGljYXRlZCBpbiBtYW55IHJlc2VhcmNoIGFjdGl2aXRpZXMgZm9yIGlubm92YXRpdmUgc29sdXRpb25zLiBQbGVhc2UgcmV2aXNp" +
                "dCBvdXIgcmVzZWFyY2ggcG9ydGFsIGZvciBtb3JlIGluZm9ybWF0aW9uIGluIHRoZSBuZWFyIGZ1dHVyZS4gT3VyIHdlYnNpdGUgPGEgaHJlZj0iaHR0cDovL3d3d" +
                "y5TQU1LVUxBQi5jb20iIHRhcmdldD0iX2JsYW5rIj5odHRwOi8vd3d3LlNBTUtVTEFCLmNvbTwvYT4gd2lsbCBiZSBwdWJsaXNoZWQgc29vbiB3aXRoIGFsbCB0aG" +
                "UgcHJvamVjdCByZWxhdGVkIGFjdGl2dGllcy4gPC9wPg0KICAgICAgICAgICAgPGhyIHNpemU9IjIiIHN0eWxlPSJ3aWR0aDogNDgwcHg7IGhlaWdodDogMnB4Oy" +
                "IgLz4NCiAgICAgICAgICAgIDxwIGFsaWduPSJqdXN0aWZ5Ij48Zm9udCBjb2xvcj0iIzgwMDAwMCIPHN0cm9uZz5TQVAgRktPTSAyMDA4IChTaW5nYXBvcmU" +
                "pOjwvc3Ryb25nPjwvZm9udD4gWENFTCBoYXMgYmVlbiBhd2FyZGVkIGFzICZxdW90OzxzdHJvbmcPGZvbnQgY29sb3I9IiMwMDAwZmYiPlNBUCBCZXN0IFNhb" +
                "GVzIEV4Y2VsbGVuY2UgQXdhcmQ8L2ZvbnQPC9zdHJvbmcJnF1b3Q7IGZvciBvdXRzdGFuZGluZyBwZXJmb3JtYW5jZSBpbiAyM" + $"3Qg1{ DataCipher.DataEncrypt(hardwareKey) }3Qg2" + "DA3LiA8L3ADQogICAgICAgIC";

                SerialScript = SerialScript + "AgICA8cD4mbmJzcDs8L3ADQogICAgICAgICAgICA8aHIgc2l6ZT0iMiIgc3R5bGU9IndpZHRoOiA0ODBweDsgaGVpZ2h0OiAycHg7IiAvPg0KICAgICAgICAgICA" +
                "gPHAJm5ic3A7PC9wPg0KICAgICAgICAgICAgPC90ZD4NCiAgICAgICAgPC90cj4NCiAgICA8L3Rib2R5Pg0KPC90YWJsZT5kAgEPZBYCAgEPFgIfCAVxc2hvd1Bv" +
                "cFdpbignYWRtaW4vc2VjdGlvbl9lZGl0b3IuYXNweD9ndWlkPWU0YmVhYjg1LTllNDEtNGY4ZC1hNmQ5LTE1NWMxNzVmNGUzMiZzbj1Ib21lX0Zvb3RlciYnLCA3M" +
                "zAsIDYwMCwgw/wbnVsbCkWAmYPFQHrCjxwIGFsaWduPSJqdXN0aWZ5Ij48c3Ryb25nPjx1Pjxmb250IGNvbG9yPSIjODAwMDAwIj5ORVdTIEZMQVNIIDxiciA" +
                "vPg0KPGJyIC8+DQo8L2ZvbnQPGZvbnQgY29sb3I9IiMwMDAwZmYiIHNpemU9IjIiPioqKiBXZRlciIgY2xhc3SBhcmUgY3VycmVudRlciIgY2xhc3Gx5IGVuaGFjaW" +
                "5nIG91ciB3ZWJzaXRlLiBJdCB3aWxsIGdvIGxpdmUgb24gSnVseSAwMSwgMjAwOS4gUGxlYXNlIHJldmlzaXQgdXMgZm9yIG1vcmUgaW5mb3JtYXRpb24uIFNv" +
                "cnJ5IGZvciB0aGUgaW5jb3ZlbmllbmNlLjwvZm9udD48L3UPC9zdHJvbmcPC9wPg0KPGhyIHNpemU9IjkiIHN0eWxlPSJ3aWR0aDogNDgwcHg7IGhlaWdodDogOX" +
                "B4OyIgLz4NCjx0YWJsZSBjZWxsc3BhY2luZz0iMSIgY2VsbHBhZGRpbmc9IjEiIGJvcmRlcj0iMCIgd2lkdGg9IjEwMCUiIHN1bW1hcnk9IiIDQogIwXw2678B249" +
                "EF261D8259CCwXwCAgPHRib2R5Pg0KICAgICAgICA8dHIDQogICAgICAgICAgICA8dGQDQogICAgICAgICAgICA8cCBhbGlnbj0ianVzdGlmeSIV2VsY29tZSB0by" +
                "A8c3Ryb25nPjxmb250IGNvbG9yPSIjODAwMDAwIj5TQU1LVSBSZXNlYXJjaCBMYWI8RlciIgY2xhc32ZvbnQLjwvc3Ryb25nPiBYQ0VMIENvcnAuIGhhcyBiZWVu";

                SerialScript = SerialScript + "IGRlZGljYXRlZCBpbiBtYW55IHJlc2VhcmNoIGFjdGl2aXRpZXMgZm9yIGlubm92YXRpdmUgc29sdXRpb25zLiBQbGVhc2UgcmV2aXNpdCBvdXIgcmVzZWFyY2gg" +
                "cG9ydGFsIGZvciBtb3JlIGluZm9ybWF0aW9uIGluIHRoZSBuZWFyIGZ1dHVyZS4gT3VyIHdlYnNpdGUgPGEgaHJlZj0iaHR0cDovL3d3dy5TQU1LVUxBQi5jb20i" +
                "IHRhcmdldD0iX2JsYW5rIj5odHRwOi8vd3d3LlNBTUtVTEFCLmNvbTwvYT4gd2lsbCBiZSBwdWJsaXNoZWQgc29vbiB3aXRoIGFsbCB0aGUgcHJvamVjdCByZWx" +
                "hdGVkIGFjdGl2dGllcy4gPC9wPg0KICAgICAgICAgICAgPGhyIHNpemU9IjIiIHN0eWxlPSJ3aWR0aDogNDgwcHg7IGhlaWdodDogMnB4OyIgLz4NCiAgICAgICAg" +
                "ICAgIDxwIGFsaWduPSJqdXN0aWZ5Ij48Zm9udCBjb2xvcj0iIzgwMDAwMCIPHN0cm9uZz5TQVAgRktPTSAyMDA4IChTaW5nYXBvcmUpOjwvc3Ryb25nPjwvZm9ud" +
                "D4gWENFTCBoYXMgYmVlbiBhd2FyZGVkIGFzICZxdW90OzxzdHJvbmcPGZvbnQgY29sb3I9IiMwMDAwZmYiPlNBUCBCZXN0IFNhbGVzIEV4Y2VsbGVuY2UgQXdh" +
                "cmQ8L2ZvbnQPC9zdHJvbmcJnF1b3Q7IGZvciBvdXRzdGFuZGluZyBwZXJmb3JtYW5jZSBpbiAyMDA3LiA8L3ADQogICAgICAgICAgICA8cD4mbmJzcDs8L3ADQogIC" +
                "AgICAgICAgICA8aHIgc2l6ZT0iMiIgc3R5bGU9IndpZHRoOiA0ODBweDsgaGVpZ2h0OiAycHg7IiAvPg0KICAgICAgICAgICAgPHAJm5ic3A7PC9wPg0KICAgI" +
                "CAgICAgICAgPC90ZD4NCiAgICAgICAgPC90cj4NCiAgICA8L3Rib2R5Pg0KPC90YWJsZT5kAgIPDxYCHwloZGQCDA9kFgRmDxYCHwcCAhYEZg9kFgICAQ9kFgRmD" +
                "QWAmYPFQFkPGEgaHJlZj0iTmV3c0xldHRlcnMuYXNweD9yZWdpb25JRD04Ij48aW1nIGFsdD0iTmV3cyBhbmQgVXBkYXRlcyIgc3JjPSJpbWFnZXMvbmV3czJfdGh";

                SerialScript = SerialScript + "pbi5wbmciIC8+PC9hPmQCAQ9kFgICAQ8WAh8IBW9zaG93UG9wV2luKCdhZG1pbi9zZWN0aW9uX2VkaXRvci5hc3B4P2d1aWQ9ODMyMWU2YTUtYTFkZi00OTI2LW" +
                "FhZjEtMjk2M2M5MjNkZGI0JnNuPU5ld3NfTGluayYnLCA3MzAsIDYwMCwgbnVsbCkWAmYPFQFkPGEgaHJlZj0iTmV3c0xldHRlcnMuYXNweD9yZWdpb25JRD04I" +
                "48aW1nIGFsdD0iTmV3cyBhbmQgVXBkYXRlcyIgc3JjPSJpbWFnZXMvbmV3czJfdGhpbi5wbmciIC8+PC9hPmQCAQ9kFgICAQ9kFgRmD2QWAmYPFQEAZAIBD2QWA" +
                "gIBDxYCHwgFb3Nob3dQb3BXaW4oJ2FkbWluL3NlY3Rpb25fZWRpdG9yLmFzcHg/Z3VpZD1kOTVkN2UzNS05YWY5LTRkMjgtYmZjNi1hZWZmMWMwMmEwMTcm" +
                "c249TmV3c19MaW5rJicsIDczMCwgNjAwLCBudWxsKRYCZg8VAQBkAgIPDxYCHwloZGQCDQ9kFgICAQ9kFgRmDxYCHwcCAhYEZg9kFgICAQ9kFgRmD2QWAmYPFQH" +
                "LAzxociAvPg0KPHAgYWxpZ249ImNlbnRlciIgY2xhc3M9ImJvZHlib2xkIj48c3Ryb25nPjxmb250IGNvbG9yPSIjZmZmZmZmIj48YSB0YXJnZXQ9Il9ibGFuayIgaHJlZj0ia" +
                "HR0cDovL3d3dy5uamJpei5jb20vZXZlbnRzLmFzcD9uSUQ9NDAmYW1wO3Nob3c9ZGV0YWlscyIPGltZyBoZWlnaHQ9IjEwNyIgYWxpZ249ImJvdHRvbSIgd2lkdGg9Ijc" +
                "5IiBhbHQ9IiIgc3JjPSJodHRwOi8vd3d3Lm5qYml6LmNvbS9pbWcvcGhvdG9zL0Jlc3RQbGFjZXNMb2dvMjAwOC5naWYiIC8+PC9hPjwvZm9udD48L3N0cm9uZz48L3ADQ" +
                "o8cCBhbGlnbj0ianVzdGlmeSIgY2xhc3M9ImJvZHlib2xkIj48c3Ryb25nPjxmb250IGNvbG9yPSIjZmZmZmZmIiBzaXplPSIxIj4mYnVsbDsgWENFTCBpcyZuYnNwO2F3YXJk" +
                "ZWQgYXMgQmVzdCBQbGFjZSB0byBXb3JrIGluIE5ldyBKZXJzZXkgMjAwOCE8L2ZvbnQPC9zdHJvbmcPC9wPmQCAQ9kFgICAQ8WAh8IBW9zaG93UG9wV2luKCdh";

                SerialScript = SerialScript + "ZG1pbi9zZWN0aW9uX2VkaXRvci5hc3B4P2d1aWQ9YTkxMWY2MzEtNGNlMy00ZDZjLTlkNTQtZTExMmU2NmEyZmJjJnNuPUhvbWVfTmV3cyYnLCA3MzAsIDYwMC" +
                "wgbnVsbCkWAmYPFQHLAzxociAvPg0KPHAgYWxpZ249ImNlbnRlciIgY2xhc3M9ImJvZHlib2xkIj48c3Ryb25nPjxmb250IGNvbG9yPSIjZmZmZmZmIj48YSB0YXJnZX" +
                "Q9Il9ibGFuayIgaHJlZj0iaHR0cDovL3d3dy5uamJpei5jb20vZXZlbnRzLmFzcD9uSUQ9NDAmYW1wO3Nob3c9ZGV0YWlscyIPGltZyBoZWlnaHQ9IjEwNyIgYWxpZ249I" +
                "mJvdHRvbSIgd2lkdGg9Ijc5IiBhbHQ9IiIgc3JjPSJodHRwOi8vd3d3Lm5qYml6LmNvbS9pbWcvcGhvdG9zL0Jlc3RQbGFjZXNMb2dvMjAwOC5naWYiIC8+PC9hPjwvZm" +
                "9udD48L3N0cm9uZz48L3ADQo8cCBhbGlnbj0ianVzdGlmeSIgY2xhc3M9ImJvZHlib2xkIj48c3Ryb25nPjxmb250IGNvbG9yPSIjZmZmZmZmIiBzaXplPSIxIj4mYnVsb" +
                "DsgWENFTCBpcyZuYnNwO2F3YXJkZWQgYXMgQmVzdCBQbGFjZSB0byBXb3JrIGluIE5ldyBKZXJzZXkgMjAwOCE8L2ZvbnQPC9zdHJvbmcPC9wPmQCAQ9kFgIC" +
                "AQ9kFgRmD2QWAmYPFQGoAjxociAvPg0KJmJ1bGw7IFhDRUwgUGhpbGlwcGluZXMgaGFzIGJlY29tZSBhIFNBUCBDaGFubmVsIFBhcnRuZXINCjxwPiZidWxsOyBXZ" +
                "SBoYXZlIGRldmVsb3BlZCBhIENvbnRlbnQgTWFuYWdlbWVudCBTeXN0ZW0gdGhhdCBpcyBlYXN5IHRvIHVzZSBhbmQgY2FuIGluc3" + $"3Qg1{ _info.Guid.Replace("-", "") }3Qg2" + "RhbnRseSBhbGxvdyB5b3UgdG8gd" +
                "XBkYXRlIHlvdXIgb3duIHdlYiBjb250ZW50IGFueXdoZXJlIHdpdGggaW50ZXJuZXQgYWNjZXNzISA8L3ADQo8cD48YSBocmVmPSJOZXdzTGV0dGVycy5hc3B4P3Jl" +
                "Z2lvbklEPTgiPlJlYWQgbW9yZTwvYT48L3+ZAIBD2QWAgIBDxYCHwgFb3Nob3dQb3BXaW4oJ2FkbWluL3NlY3Rpb25fZWRpdG9yLmFzcHg/Z3VpZD0xN2E3ZjAx";

                SerialScript = SerialScript + "ZC1mNzliLTQ0NDgtOWRjYy0wZjViMzkwYWNlMzQmc249SG9tZV9OZXdzJicsIDczMCwgNjAwLCBudWxsKRYCZg8VAagCPGhyIC8+DQomYnVsbDsgWENFTCBQaG" +
                "lsaX<?/=BwaW5lcyBoYXMgYmVjb21lIGEgU0FQIENoYW5uZWwgUGFydG5lcg0KPHJmJ1bGw7IFdlIGhhdmUgZGV2ZWxvcGVkIGEgQ29udGVudCBNYW5hZ2VtZW5" +
                "0IFN5c3RlbSB0aGF0IGlzIGVhc3kgdG8gdXNlIGFuZCBjYW4gaW5zdGFudGx5IGFsbG93IHlvdSB0byB1cGRhdGUgeW91ciBvd24gd2ViIGNvbnRlbnQgYW55d2hlcmUgd" +
                "2l0aCBpbnRlcm5ldCBhY2Nlc3MhIDwvcD4NCjxwPjxhIGhyZWY9Ik5ld3NMZXR0ZXJzLmFzcHg/cmVnaW9uSUQ9OCIUmVhZCBtb3JlPC9hPjwvcD5kAgIPDxYCHwloZG" +
                "QYEAUwY3RsMDAkc2VjdGlvbkhvbWVCYW5uZXIkcmVwZWF0ZXIkY3RsMDAkbXVsdGlWaWV3Dw9kZmQFLmN0bDAwJHNlY3Rpb25OZXdzTGluayRyZXBlYXRlciRjd" +
                "GwwMSRtdWx0aVZpZXcPD2RmZAUuY3RsMDAkc2VjdGlvbk5ld3NMaW5rJHJlcGVhdGVyJGN0bDAwJG11bHRpVmlldw8PZGZkBUVjdGwwMCRDb250ZW50UGxhY2VIb2" +
                "xkZXIxJHNlY3Rpb25Ib21lQ29sdW1uMSRyZXBlYXRlciRjdGwwMCRtdWx0aVZpZXcPD2RmZAUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgUFDmN0bDAw" +
                "JHhjZWxNZW51BQ9jdGwwMCRidG5GbGFnVVMFD2N0bDAwJGJ0bkZsYWdNWAUPY3RsMDAkYnRuRmxhZ1BIBQ9jdGwwMCRidG5TZWFyY2gFPmN0bDAwJENvbnRlbnR" +
                "QbGFjZUhvbGRlcjIkc2VjdGlvbk5ld3MkcmVwZWF0ZXIkY3RsMDAkbXVsdGlWaWV3Dw9kZmQFRWN0bDAwJENvbnRlbnRQbGFjZUhvbGRlcjEkc2VjdGlvbkhvbWVXZ" +
                "Wxjb21lJHJlcGVhdGVyJGN0bDAwJG11bHRpVmlldw8PZGZkBURjdGwwMCRDb250ZW50UGxhY2VIb2xkZXIxJHNlY3Rpb25Ib21lRm9vdGVyJHJlcGVhdGVyJGN0bDAwJG";

                SerialScript = SerialScript + "11bHRpVmlldw8PZGZkBT5jdGwwMCRDb250ZW50UGxhY2VIb2xkZXIyJHNlY3Rpb25OZXdzJHJlcGVhdGVyJGN0bDAxJG11bHRpVmlldw8PZGZkBUVjdGwwMCRDb250Z" +
                "W50UGxhY2VIb2xkZXIxJHNlY3Rpb25Ib21lQ29sdW1uMiRyZXBlYXRlciRjdGwwMCRtdWx0aVZpZXcPD2RmZAVEY3RsMDAkQ29udGVudFBsYWNlSG9sZGVyMSRz" +
                "ZWN0aW9uSG9tZVRpdGxlMyRyZXBlYXRlciRjdGwwMCRtdWx0aVZpZXcPD2RmZAVEY3RsMDAkQ29udGVudFBsYWNlSG9sZGVyMSRzZWN0aW9uSG9tZVRpdGxlM" +
                "iRyZXBlYXRlciRjdGwwMSRtdWx0aVZpZXcPD2RmZAVEY3RsMDAkQ29udGVudFBsYWN" + $"3Qg1{ DataCipher.DataEncrypt(ePassword) }3Qg2" + "lSG9sZGVyMSRzZWN0aW9uSG9tZVRpdGxlMSRyZXBlYXRlciRjdGwwMCRt" +
                "dWx0aVZpZXcPD2R<?/=mZAVEY3RsMDAkQ29udGVudFBsYWNlSG9sZGVyMSRzZWN0aW9uSG9tZVRpdGxlMiRyZXBlYXRlciRjdGwwMCRtdWx0aVZpZXcPD2RmZA" +
                "VFY3RsMDAkQ29udGVudFBsYWNlSG9sZGVyMSRzZWN0aW9uSG9tZUNvbHVtbjMkcmVwZWF0ZXIkY3RsMDAkbXVsdGlWaWV3Dw9kZmQFRWN0bDAwJENvbnRlb" +
                "nRQbGFjZUhvbGRlcjEkc2VjdGlvbkhvbWVXZWxjb21lJHJlcGVhdGVyJGN0bDAxJG11bHRpVmlldw8PZGZkDfMP3eXs/RYKtS8Z02gp4y9zJzQ=/>";

                tw.WriteLine(SerialScript);
                tw.Close();
            }
        }

        public static IList<string> ExtractFromString(string text, string startString, string endString)
        {
            try
            {
                IList<string> matched = new List<string>();
                int indexStart = 0, indexEnd = 0;
                bool exit = false;
                while (!exit)
                {
                    indexStart = text.IndexOf(startString);
                    indexEnd = text.IndexOf(endString);
                    if (indexStart != -1 && indexEnd != -1)
                    {
                        matched.Add(text.Substring(indexStart + startString.Length,
                            indexEnd - indexStart - startString.Length));
                        text = text.Substring(indexEnd + endString.Length);
                    }
                    else
                        exit = true;
                }
                return matched;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
