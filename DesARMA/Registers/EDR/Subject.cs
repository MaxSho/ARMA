using Microsoft.Extensions.Options;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace DesARMA.Registers.EDR
{
    public class Subject
    {
        public int? id { get; set; }
        public int? state { get; set; }
        public string? state_text { get; set; }
        public string? code { get; set; }
        public Names? names { get; set; }
        public string? olf_code { get; set; }
        public string? olf_name { get; set; }
        public string? olf_subtype { get; set; }
        public string? founding_document { get; set; }
        public Executive_power? executive_power { get; set; }
        public string? object_name { get; set; }
        public List<Founder>? founders { get; set; }
        public Authorised_capital? authorised_capital { get; set; }
        public string? management { get; set; }
        public List<Head?>? heads { get; set; }
        public string? managing_paper { get; set; }
        public bool? is_modal_statute { get; set; }
        public List<ActivityKind?>? activity_kinds { get; set; }
        public List<Branch?>? branches { get; set; }
        public Address? address { get; set; }
        public Registration2? registration { get; set; }
        public Bankruptcy? bankruptcy { get; set; }
        public Termination? termination { get; set; }
        public Termination_cancel? termination_cancel { get; set; }
        public List<RelatedSubject?>? assignees { get; set; }
        public List<RelatedSubject?>? predecessors { get; set; }
        public List<Registration?>? registrations { get; set; }
        public PrimaryActivityKind? primary_activity_kind { get; set; }
        public string? prev_registration_end_term { get; set; }
        public List<string?>? open_enforcements { get; set; }
        public Contacts? contacts { get; set; }
    }
    public class RelatedSubject
    {
        public string? name { get; set; }
        public string? code { get; set; }
        public Address? address { get; set; }
        public string? last_name { get; set; }
        public string? first_middle_name { get; set; }
        public int? role { get; set; }
        public string? role_text { get; set; }
        public int? id { get; set; }
        public string? url { get; set; }
    }
    public class Termination_cancel
    {
        public string? date { get; set; }
        public string? record_number { get; set; }
        public string? doc_number { get; set; }
        public string? doc_date { get; set; }
        public string? date_judge { get; set; }
        public string? court_name { get; set; }

    }
    public class Termination
    {
        public string? date { get; set; }
        public int? state { get; set; }
        public string? state_text { get; set; }
        public string? record_number { get; set; }
        public string? requirement_end_date { get; set; }
        public string? cause { get; set; }
    }
    public class Bankruptcy
    {
        public string? date { get; set; }
        public int? state { get; set; }
        public string? state_text { get; set; }
        public string? doc_number { get; set; }
        public string? doc_date { get; set; }
        public string? date_judge { get; set; }
        public string? court_name { get; set; }
    }
    public class Branch
    {
        public string? name { get; set; }
        public string? code { get; set; }
        public int? role { get; set; }
        public string? role_text { get; set; }
        public int? type { get; set; }
        public string? type_text { get; set; }
        public List<ActivityKind?>? activity_kinds { get; set; }
        public List<Head?>? heads { get; set; }
        public string? create_date { get; set; }
        public Address? address { get; set; }
        public Contacts? contacts { get; set; }
    }
    public class Head
    {
        public string? name { get; set; }
        public string? code { get; set; }
        public Address? address { get; set; }
        public string? last_name { get; set; }
        public string? first_middle_name { get; set; }
        public int? role { get; set; }
        public string? role_text { get; set; }
        public string? position { get; set; }
        public int? id { get; set; }
        public string? url { get; set; }
        public string? appointment_date { get; set; }
        public string? restriction { get; set; }
    }
    public class Authorised_capital
    {
        public double? value { get; set; }
        public string? date { get; set; }
    }
    public class Executive_power
    {
        public string? name { get; set; }
        public string? code { get; set; }
    }
    public class Founder
    {
        public string? name { get; set; }
        public string? code { get; set; }
        public string? country { get; set; }
        public Address? address { get; set; }
        public string? last_name { get; set; }
        public string? first_middle_name { get; set; }
        public int? role { get; set; }
        public string? role_text { get; set; }
        public int? id { get; set; }
        public string? url { get; set; }
        public double? capital { get; set; }
    }
    public class ActivityKind
    {
        public string? name { get; set; }
        public string? code { get; set; }
        public bool? is_primary { get; set; }
    }
    public class Address
    {
        public string? zip { get; set; }
        public string? country { get; set; }
        public string? address { get; set; }
        public Parts? parts { get; set; }
    }
    public class Contacts
    {
        public string? email { get; set; }
        public List<string?>? tel { get; set; }
        public string? fax { get; set; }
        public string? web_page { get; set; }
    }
    public class Names
    {
        public string? name { get; set; }
        public int? include_olf { get; set; }
        public string? display { get; set; }
        public string? @short { get; set; }
        public string? name_en { get; set; }
        public string? short_en { get; set; }
    }
    public class Parts
    {
        public string? atu { get; set; }
        public string? atu_code { get; set; }
        public string? street { get; set; }
        public string? house_type { get; set; }
        public string? house { get; set; }
        public string? building_type { get; set; }
        public string? building { get; set; }
        public string? num_type { get; set; }
        public string? num { get; set; }
    }
    public class PrimaryActivityKind
    {
        public string? name { get; set; }
        public string? code { get; set; }
        public string? reg_number { get; set; }
        public string? @class { get; set; }
    }
    public class Registration2
    {
        public string? date { get; set; }
        public string? record_number { get; set; }
        public string? record_date { get; set; }
        public bool? is_separation { get; set; }
        public bool? is_division { get; set; }
        public bool? is_merge { get; set; }
        public bool? is_transformation { get; set; }
    }
    public class Registration
    {
        public string? name { get; set; }
        public string? code { get; set; }
        public int? type { get; set; }
        public string? description { get; set; }
        public string? reg_number { get; set; }
        public string? start_date { get; set; }
        public string? start_num { get; set; }
        public string? end_date { get; set; }
        public string? end_num { get; set; }
        public object? @class { get; set; }
    }
    
}
