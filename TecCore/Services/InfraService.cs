﻿using TecCore.Models;

namespace TecCore.Services;

public class InfraService
{
    
    
    public InfraService()
    {
        LoadRooms();
    }
    
    public List<RoomLocation> GetRoomLocations()
    {
        return RoomLocations;
    }
    
    public List<RoomLocation> RoomLocations { get; set; } = [];

    private void LoadRooms()
    {
        RoomLocations.Add(new RoomLocation { RoomNumber = "000", Name = "General/Other" });
        RoomLocations.Add(new RoomLocation { RoomNumber = "400", Name = "Digital Media" });
        RoomLocations.Add(new RoomLocation
        {
            RoomNumber = "410", Name = "IT Office",
            Labs =
            [
                new RoomLocation() { RoomNumber = "603", Name = "IT Shop" },
                new RoomLocation() { RoomNumber = "602", Name = "Computer Lab" }
            ]
        });
        RoomLocations.Add(new RoomLocation
        {
            RoomNumber = "420", Name = "Administrative Offices",
            Labs =
            [
                new RoomLocation { RoomNumber = "420", Name = "Front Desk" },
                new RoomLocation { RoomNumber = "420", Name = "Coop" },
                new RoomLocation { RoomNumber = "420", Name = "Principal" },
                new RoomLocation { RoomNumber = "409", Name = "Conference Room" },
                new RoomLocation { RoomNumber = "420", Name = "Counselling" },
                new RoomLocation { RoomNumber = "420", Name = "Student Services" },
                new RoomLocation { RoomNumber = "420", Name = "Coop Office" }
            ]
        });
        RoomLocations.Add(new RoomLocation
        {
            RoomNumber = "501", Name = "Marketing",
            Labs = [new RoomLocation() { RoomNumber = "500", Name = "School Store" }]
        });
        RoomLocations.Add(new RoomLocation
        {
            RoomNumber = "503", Name = "Patient Services",
            Labs =
            [
                new RoomLocation() { RoomNumber = "504", Name = "Patient Services - Lab" }
            ]
        });
        RoomLocations.Add(new RoomLocation
        {
            RoomNumber = "507", Name = "Interior Design",
            Labs = [new RoomLocation() { RoomNumber = "713", Name = "Interior Design - Lab" }]
        });
        RoomLocations.Add(new RoomLocation
        {
            RoomNumber = "508", Name = "Technical Drafting",
            Labs =
            [
                new RoomLocation { RoomNumber = "509", Name = "Tech Drafting - Lab" }
            ]
        });
        RoomLocations.Add(new RoomLocation { RoomNumber = "511", Name = "Health Careers" });
        RoomLocations.Add(new RoomLocation { RoomNumber = "600", Name = "Accounting" });
        RoomLocations.Add(new RoomLocation { RoomNumber = "601", Name = "Educational Careers" });
        RoomLocations.Add(new RoomLocation { RoomNumber = "604", Name = "Cybersecurity" });
        RoomLocations.Add(new RoomLocation { RoomNumber = "605", Name = "Business Management" });
        RoomLocations.Add(new RoomLocation
        {
            RoomNumber = "610", Name = "Culinary Arts",
            Labs =
            [
                new RoomLocation() { RoomNumber = "611", Name = "Culinary Arts - Office 1" },
                new RoomLocation() { RoomNumber = "612", Name = "Culinary Arts - Office 2" },
                new RoomLocation() { RoomNumber = "613", Name = "Culinary Arts - Kitchen" }
            ]
        });
        RoomLocations.Add(new RoomLocation { RoomNumber = "614", Name = "Electronics" });
        RoomLocations.Add(new RoomLocation
        {
            RoomNumber = "620", Name = "Small Engines",
            Labs =
            [
                new RoomLocation() { RoomNumber = "621", Name = "Small Engines - Lab" },
                new RoomLocation() { RoomNumber = "622", Name = "Small Engines - Bay" }
            ]
        });
        RoomLocations.Add(new RoomLocation { RoomNumber = "700", Name = "Machine Trades" });
        RoomLocations.Add(new RoomLocation
        {
            RoomNumber = "701", Name = "Graphic Arts",
            Labs =
            [
                new RoomLocation() { RoomNumber = "510", Name = "Graphic Arts - Lab" }
            ]
        });
        RoomLocations.Add(new RoomLocation
        {
            RoomNumber = "705", Name = "Automotive Technology",
            Labs =
            [
                new RoomLocation { RoomNumber = "704", Name = "Auto Tech - Lab" }
            ]
        });
        RoomLocations.Add(new RoomLocation { RoomNumber = "710", Name = "Welding" });
        RoomLocations.Add(new RoomLocation
        {
            RoomNumber = "711", Name = "Construction Trades",
            Labs =
            [
                new RoomLocation() { RoomNumber = "712", Name = "Wood Shop" },
                new RoomLocation() { RoomNumber = "714", Name = "Lab 1" },
                new RoomLocation() { RoomNumber = "715", Name = "Lab 2" },
                new RoomLocation() { RoomNumber = "716", Name = "Lab 3" }
            ]
        });
        RoomLocations.Add(new RoomLocation { RoomNumber = "999", Name = "TEST LOCATION" });
    }
    
    
}