using DesARMA.Registers.EDR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using DesARMA.ModelCentextEDR;

namespace DesARMA.Registers.EDR
{
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdSubject { get; set; }
        public decimal id { get; set; }
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

        [NotMapped]
        [JsonProperty("beneficiaries")]
        [JsonConverter(typeof(BeneficiariesConverter))]
        public object? beneficiaries { get; set; }
        public List<Beneficiaries?>? subBeneficiar
        {
            get
            {
                if (beneficiaries is List<Beneficiaries?> list)
                    return list;
                return null;
            }
            set
            {
                if (value is List<Beneficiaries?> list)
                    beneficiaries = list;
            }
        }
        public Reason? reasons
        {
            get
            {
                if (beneficiaries is Reason reason)
                    return reason;
                return null;
            }
            set
            {
                if (value is Reason reason)
                    beneficiaries = reason;
            }
        }
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
        [NotMapped]
        public List<string?>? open_enforcements { get; set; }


    }
    public class Beneficiaries
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdBeneficiar { get; set; }
        public string? name { get; set; }
        public string? code { get; set; }
        public string? country { get; set; }
        public Address? address { get; set; }
        public string? last_name { get; set; }
        public string? first_middle_name { get; set; }
        public decimal? beneficiaries_type { get; set; }
        public decimal? role { get; set; }
        public string? role_text { get; set; }
        public decimal? interest { get; set; }
    }
    public class Reason
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdReason { get; set; }
        public string? reason { get; set; }
    }
    public class RelatedSubject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdRelatedSubject { get; set; }
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdBranch { get; set; }
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdHead { get; set; }
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdFounder { get; set; }
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdActivityKind { get; set; }
        public string? name { get; set; }
        public string? code { get; set; }
        public bool? is_primary { get; set; }
    }
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdAddress { get; set; }
        public string? zip { get; set; }
        public string? country { get; set; }
        public string? address { get; set; }
        public Inline_model_8? parts { get; set; } // inline_model_8
    }
    public class Contacts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdContact { get; set; }
        public string? email { get; set; }
        [NotMapped]
        public List<string?>? tel { get; set; }
        public string? fax { get; set; }
        public string? web_page { get; set; }
    }
    public class Inline_model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdInline_model { get; set; }
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdInline_model0 { get; set; }
        public string? name { get; set; }
        public string? code { get; set; }
    }
    public class Inline_model_1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdInline_model1 { get; set; }
        public double? value { get; set; }
        public string? date { get; set; }
    }
    public class Inline_model_2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdInline_model2 { get; set; }
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdInline_model3 { get; set; }
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdInline_model4 { get; set; }
        public string? date { get; set; }
        public decimal? state { get; set; }
        public string? state_text { get; set; }
        public string? record_number { get; set; }
        public string? requirement_end_date { get; set; }
        public string? cause { get; set; }
    }
    public class Inline_model_5
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdInline_model5 { get; set; }
        public string? date { get; set; }
        public string? record_number { get; set; }
        public string? doc_number { get; set; }
        public string? doc_date { get; set; }
        public string? date_judge { get; set; }
        public string? court_name { get; set; }

    }
    public class Inline_model_6
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdInline_model6 { get; set; }
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdInline_model7 { get; set; }
        public string? name { get; set; }
        public string? code { get; set; }
        public string? reg_number { get; set; }
        public string? @class { get; set; }
    }
    public class Inline_model_8
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IdInline_model8 { get; set; }
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
    public class BeneficiariesConverter : JsonConverter
    {
        private readonly DbContext _dbContext;

        public BeneficiariesConverter(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public BeneficiariesConverter()
        {
            _dbContext = new ModelContextEDR();
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(object));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            JObject jsonObject;


            if (reader.TokenType == JsonToken.StartObject)
            {
                jsonObject = JObject.Load(reader);
                Reason reason = jsonObject.ToObject<Reason>()!;
                return reason;

            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                var array = JArray.Load(reader);
                // обробляємо масив тут, наприклад:
                var beneficiaries = array.ToObject<List<Beneficiaries>>();
                return beneficiaries;
            }
            else
            {
                throw new JsonReaderException("Unexpected token type: " + reader.TokenType);
            }

            // обробляємо об'єкт JSON тут

            var subject = new Subject();
            subject.beneficiaries = jsonObject;
            serializer.Populate(jsonObject.CreateReader(), subject);

            return subject;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
