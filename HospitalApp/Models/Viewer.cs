using Microsoft.Data.SqlClient;

namespace HospitalApp.Models
{
    // Represents a registered visitor for an admitted patient, with an IsAllowed flag controlled by the patient's doctor.
    public class Viewer
    {
        public int ViewerID {get; set;}
        public int AdmissionID {get; set;}
        public string ViewerName {get; set;} = string.Empty;
        public string? Relation {get; set;}
        public string? Phone {get; set;}
        public bool IsAllowed {get; set;} = true;

        // Constructs a Viewer instance from the current row of a SqlDataReader.
        public static Viewer FromReader(SqlDataReader reader) => new()
        {
            ViewerID = (int)reader["ViewerID"],
            AdmissionID = (int)reader["AdmissionID"],
            ViewerName = (string)reader["ViewerName"],
            Relation = reader["Relation"] as string,
            Phone = reader["Phone"] as string,
            IsAllowed = (bool)reader["IsAllowed"]
        };
    }
}