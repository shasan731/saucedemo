# Automation testing assignment for OSTAD

# SauceDemo UI Tests (C# / .NET 8)

I used C# and .NET 8 because I have some experience coding in C#.

UI tests for saucedemo.com using xUnit + Selenium (Chrome) + FluentAssertions + Allure.
Pattern: Page Objects (per screen) + a ScenarioHelper (strings steps together).

# Run the project

dotnet restore

dotnet test

Chrome must be installed.

# What the tests do

Q1: Login with locked_out_user → expect error message.

Q2: standard_user → add 3 items → checkout → verify names & totals → finish → reset & logout.

Q3: performance_glitch_user → reset → sort Z→A → add first item → checkout → verify → finish → reset & logout.

Then it runs Q1, Q2, Q3 back-to-back.

# Allure report

Results are written automatically to:

bin/Debug/net8.0/allure-results

# Test data

Users: standard_user, locked_out_user, performance_glitch_user

Password: secret_sauce

# Test device

I ran the project on MacOS and Windows 11 (Arm version installed in VMWare Fusion on MacOS).
Test ran on both OS but took a bit longer for the first run in Windows 11.
