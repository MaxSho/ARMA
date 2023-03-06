using Microsoft.Extensions.Options;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;

namespace DesARMA.Registers.EDR
{
    public class Subject
    {
        public decimal? id { get; set; }
        public decimal? state { get; set; }
        public string? state_text { get; set; }
        public string? code { get; set; }
        public string? country { get; set; }
        public Inline_model? names { get; set; } // inline_model
        public string? olf_code { get; set; }
        public string? olf_name { get; set; }
        public string? olf_subtype { get; set; }
        public string? founding_document { get; set; }
        public decimal? founding_document_type { get; set; }
        public string? founding_document_name { get; set; }
        public Inline_model_0? executive_power { get; set; } // inline_model_0
        public string? object_name { get; set; }
        public List<Founder?>? founders { get; set; }
        public List<Beneficiaries?>? beneficiaries { get; set; } //(Array[Beneficiary] або Reason, optional)
        public List<Branch?>? branches { get; set; }
        public Inline_model_1? authorised_capital { get; set; } // inline_model_1
        public string? management { get; set; }
        public string? managing_paper { get; set; }
        public bool? is_modal_statute { get; set; }
        public List<ActivityKind?>? activity_kinds { get; set; }
        public List<Head?>? heads { get; set; }
        public Address? address { get; set; }
        public Inline_model_2? registration { get; set; } //inline_model_2
        public Inline_model_3? bankruptcy { get; set; } //inline_model_3
        public Inline_model_4? termination { get; set; } //inline_model_4
        public Inline_model_5? termination_cancel { get; set; } //inline_model_5
        public List<RelatedSubject?>? assignees { get; set; }
        public List<RelatedSubject?>? predecessors { get; set; }
        public List<Inline_model_6?>? registrations { get; set; }  // inline_model_6
        public Inline_model_7? primary_activity_kind { get; set; } // inline_model_7
        public string? prev_registration_end_term { get; set; }
        public Contacts? contacts { get; set; }
        public List<string?>? open_enforcements { get; set; }
    }
    public class Beneficiaries
    {
        public string? reason { get; set; }
        public string? name { get; set; }
        public string?  code { get; set; }
        public string? country { get; set; }
        public Address? address { get; set; }
        public string? last_name { get; set; }
        public string? first_middle_name { get; set; }
        public decimal? beneficiaries_type { get; set; }
        public decimal? role { get; set; }
        public string?  role_text { get; set; }
        public float? decimalerest { get; set; }

    }
    public class RelatedSubject
    {
        public string? name { get; set; }
        public string? code { get; set; }
        public Address? address { get; set; }
        public string? last_name { get; set; }
        public string? first_middle_name { get; set; }
        public decimal? role { get; set; }
        public string? role_text { get; set; }
        public decimal? id { get; set; }
        public string? url { get; set; }
    }
    public class Branch
    {
        public string? name { get; set; }
        public string? code { get; set; }
        public decimal? role { get; set; }
        public string? role_text { get; set; }
        public decimal? type { get; set; }
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
        public decimal? role { get; set; }
        public string? role_text { get; set; }
        public string? position { get; set; }
        public decimal? id { get; set; }
        public string? url { get; set; }
        public string? appointment_date { get; set; }
        public string? restriction { get; set; }
    }
    public class Founder
    {
        public string? name { get; set; }
        public string? code { get; set; }
        public string? country { get; set; }
        public Address? address { get; set; }
        public string? last_name { get; set; }
        public string? first_middle_name { get; set; }
        public decimal? role { get; set; }
        public string? role_text { get; set; }
        public decimal? id { get; set; }
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
        public Inline_model_8? parts { get; set; } // inline_model_8
    }
    public class Contacts
    {
        public string? email { get; set; }
        public List<string?>? tel { get; set; }
        public string? fax { get; set; }
        public string? web_page { get; set; }
    }
    
    public class Inline_model
    {
        public string? name { get; set; }
        public decimal? include_olf { get; set; }
        public string? display { get; set; }
        public string? @short { get; set; }
        public string? name_en { get; set; }
        public string? short_en { get; set; }
        public string? name_for { get; set; }
        public string? short_for { get; set; }

    }
    public class Inline_model_0
    {
        public string? name { get; set; }
        public string? code { get; set; }
    }
    public class Inline_model_1
    {
        public double? value { get; set; }
        public string? date { get; set; }
    }
    public class Inline_model_2
    {
        public string? date { get; set; }
        public string? record_number { get; set; }
        public string? record_date { get; set; }
        public bool? is_separation { get; set; }
        public bool? is_division { get; set; }
        public bool? is_merge { get; set; }
        public bool? is_transformation { get; set; }
    }
    public class Inline_model_3
    {
        public string? date { get; set; }
        public decimal? state { get; set; }
        public string? state_text { get; set; }
        public string? doc_number { get; set; }
        public string? doc_date { get; set; }
        public string? date_judge { get; set; }
        public string? court_name { get; set; }
    }
    public class Inline_model_4
    {
        public string? date { get; set; }
        public decimal? state { get; set; }
        public string? state_text { get; set; }
        public string? record_number { get; set; }
        public string? requirement_end_date { get; set; }
        public string? cause { get; set; }
    }
    public class Inline_model_5
    {
        public string? date { get; set; }
        public string? record_number { get; set; }
        public string? doc_number { get; set; }
        public string? doc_date { get; set; }
        public string? date_judge { get; set; }
        public string? court_name { get; set; }

    }
    public class Inline_model_6
    {
        public string? name { get; set; }
        public string? code { get; set; }
        public decimal? type { get; set; }
        public string? description { get; set; }
        public string? reg_number { get; set; }
        public string? start_date { get; set; }
        public string? start_num { get; set; }
        public string? end_date { get; set; }
        public string? end_num { get; set; }
    }
    public class Inline_model_7
    {
        public string? name { get; set; }
        public string? code { get; set; }
        public string? reg_number { get; set; }
        public string? @class { get; set; }
    }
    public class Inline_model_8
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

}
