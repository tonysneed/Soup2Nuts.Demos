Getting Started with ASP.NET 5 (vNext) ReadMe

GitHub Home Repo:
https://github.com/aspnet/home

Inspect the following two files in HelloWeb directory:
- project.json
- startup.cs

1. Install the KVM (K Version Manager)
   - Run following from admin prompt:
     @powershell -NoProfile -ExecutionPolicy unrestricted -Command "iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/master/kvminstall.ps1'))"
 
2. Install latest K Runtime
   - Re-open the command prompt, then run the following:
     kvm upgrade
   - List the installed runtime versions:
     kvm list

3. Restore NuGet packages
   - Navigate to HelloWeb directory and run:
     kpm restore
     
4. Run the web command:
   k web
   
5. Open a browser and navigate to:
   http://localhost:5001
   - You should see the ASP.NET vNext welcome page
   
