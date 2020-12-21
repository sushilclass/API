using System;
using System.Collections.Generic;
using System.Text;

namespace Outreach_FRMS_Model
{
    public class SearchAPIResponse
    {
#pragma warning disable CA2227 // Collection properties should be read only
        public IList<object> html_attributions { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
#pragma warning disable CA2227 // Collection properties should be read only
        public IList<Result> results { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
        public string status { get; set; }
        public string next_page_token { get; set; }
    }
    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Northeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southwest
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Viewport
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Geometry
    {
        public Location location { get; set; }
        public Viewport viewport { get; set; }
    }

    public class OpeningHours
    {
        public bool open_now { get; set; }
    }

    public class Photo
    {
        public int height { get; set; }
#pragma warning disable CA2227 // Collection properties should be read only
        public IList<string> html_attributions { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
        public string photo_reference { get; set; }
        public int width { get; set; }
    }

    public class PlusCode
    {
        public string compound_code { get; set; }
        public string global_code { get; set; }
    }

    public class Result
    {
        public string business_status { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public OpeningHours opening_hours { get; set; }
#pragma warning disable CA2227 // Collection properties should be read only
        public IList<Photo> photos { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
        public string place_id { get; set; }
        public PlusCode plus_code { get; set; }
        public double rating { get; set; }
        public string reference { get; set; }
#pragma warning disable CA2227 // Collection properties should be read only
        public IList<string> types { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
        public int user_ratings_total { get; set; }
    }

    public class SaveResult
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }
        public string OpenNow { get; set; }
        public string IconImage { get; set; }
        public string Type { get; set; }
    }
}
