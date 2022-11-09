using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace FhirBlaze.AllergyIntoleranceModule.Components
{
  public partial class AllergyIntoleranceDetailComponent
  {
    [Parameter]
    public EventCallback<AllergyIntolerance> OnAllergyIntoleranceSaved { get; set; }

    [Parameter]
    public bool IsEditable { get; set; } = true;

    public bool Processing { get; set; } = false;

    [Parameter]
    public List<Patient> Patients { get; set; } = new List<Patient>();

    [Parameter]
    public List<Practitioner> Practitioners { get; set; } = new List<Practitioner>();

    [CascadingParameter]
    public AllergyIntolerance AllergyIntolerance { get; set; } = new AllergyIntolerance();

    public string[] ClinicalStatuses = Enum.GetNames(typeof(AllergyIntolerance.AllergyIntoleranceClinicalStatusCodes));

    public string[] VerificationStatuses = Enum.GetNames(typeof(AllergyIntolerance.AllergyIntoleranceVerificationStatusCodes));

    public string ClinicalStatus
    {
      get
      {
        if (this.AllergyIntolerance.ClinicalStatus != null && this.AllergyIntolerance.ClinicalStatus.Coding.Count > 0)
        {
          return $"{this.AllergyIntolerance.ClinicalStatus.Coding[0].Display}";
        }

        return string.Empty;
      }
      set
      {
        try
        {
          AllergyIntolerance.AllergyIntoleranceClinicalStatusCodes statusCode;

          if (Enum.TryParse<AllergyIntolerance.AllergyIntoleranceClinicalStatusCodes>(value, true, out statusCode))
          {
            Console.WriteLine($"Status value set: {value}");
            this.AllergyIntolerance.ClinicalStatus = GeneratedCodeConcept(value.ToLower(), value);
          }
          else
          {
            Console.WriteLine("Status value not set: " + value);
          }
        }
        catch (Exception e)
        {
          Console.WriteLine("Error in setClinicalStatus from AllergyIntoleranceDetailComponent.razor.cs: " + e.Message + "; Source: " + e.Source + "; StackTrace: " + e.StackTrace);
        }
      }
    }
    public string VerificationStatus
    {
      get
      {
        if (this.AllergyIntolerance.VerificationStatus != null && this.AllergyIntolerance.VerificationStatus.Coding.Count > 0)
        {
          return $"{this.AllergyIntolerance.VerificationStatus.Coding[0].Display}";
        }

        return string.Empty;
      }
      set
      {
        try
        {
          AllergyIntolerance.AllergyIntoleranceVerificationStatusCodes statusCode;

          if (Enum.TryParse<AllergyIntolerance.AllergyIntoleranceVerificationStatusCodes>(value, true, out statusCode))
          {
            Console.WriteLine($"Status value set: {value}");
            this.AllergyIntolerance.VerificationStatus = GeneratedCodeConcept(value.ToLower(), value);
          }
          else
          {
            Console.WriteLine("Status value not set: " + value);
          }
        }
        catch (Exception e)
        {
          Console.WriteLine("Error in setVerificationStatus from AllergyIntoleranceDetailComponent.razor.cs: " + e.Message + "; Source: " + e.Source + "; StackTrace: " + e.StackTrace);
        }
      }
    }

    public string SelectedType
    {
      get
      {
        return this.AllergyIntolerance.Type.ToString();
      }
      set
      {
        this.AllergyIntolerance.Type = new AllergyIntolerance.AllergyIntoleranceType();
      }
    }

    public string RecorderType { get; set; }

    public string SelectedPatientId
    {
      get
      {
        try
        {
          if (this.AllergyIntolerance.Patient != null)
          {
            foreach (var patient in this.Patients)
            {
              if ($"Patient/{patient.Id}" == this.AllergyIntolerance.Patient.Reference)
              {
                return patient.Id;
              }
            }
          }
        }
        catch (System.Exception e)
        {
          Console.WriteLine("Error in getSelectedPatientId from AllergyIntoleranceDetailComponent.razor.cs: " + e.Message + "; Source: " + e.Source + "; StackTrace: " + e.StackTrace);
        }

        return "0";
      }
      set
      {
        try
        {
          var display = this.Patients[this.patientIndexFromId(value)].Name[0].ToString();
          this.AllergyIntolerance.Patient = new ResourceReference($"Patient/{value}", display);
        }
        catch (System.Exception e)
        {
          Console.WriteLine("Error in setSelectedPatientId from AllergyIntoleranceDetailComponent.razor.cs: " + e.Message + "; Source: " + e.Source + "; StackTrace: " + e.StackTrace);
        }
      }
    }

    public string SelectedRecorder
    {
      get
      {
        if (this.AllergyIntolerance.Recorder != null)
        {
          return this.AllergyIntolerance.Recorder.Reference;
        }

        return string.Empty;
      }
      set
      {
        this.AllergyIntolerance.Recorder = new ResourceReference(value);
        this.AllergyIntolerance.Recorder.Type = this.RecorderType;
      }
    }

    public DateTime SelectedRecordedDate
    {
      get
      {
        if (this.AllergyIntolerance.RecordedDate != null && !string.IsNullOrWhiteSpace(this.AllergyIntolerance.RecordedDate.ToString()))
        {
          return DateTime.Parse(this.AllergyIntolerance.RecordedDate.ToString());
        }

        return DateTime.Now;
      }
      set => this.AllergyIntolerance.RecordedDate = value.ToString("yyyy-MM-dd");
    }

    public string Annote
    {
      get
      {
        if (this.AllergyIntolerance.Note != null && this.AllergyIntolerance.Note.Count > 0)
        {
          return this.AllergyIntolerance.Note[0].Text.ToString();
        }

        return string.Empty;
      }
      set
      {
        if (this.AllergyIntolerance.Note != null && this.AllergyIntolerance.Note.Count > 0)
        {
          this.AllergyIntolerance.Note[0].Text = new Markdown(value);
        }
        else
        {
          Annotation note = new Annotation();
          note.Text = new Markdown(value);
          this.AllergyIntolerance.Note.Add(note);
        }
      }
    }

    public void AddNoteAnnotation()
    {
      if (this.AllergyIntolerance.Note == null)
      {
        this.AllergyIntolerance.Note = new List<Annotation>();
      }

      this.AllergyIntolerance.Note.Add(new Annotation() { Text = new Markdown() });
    }

    public void RemoveNoteAnnotation(Annotation note)
    {
      this.AllergyIntolerance.Note.Remove(note);
    }

    private int patientIndexFromId(string Id = null)
    {
      for (var index = 0; index < this.Patients.Count; index++)
      {
        if (Id == this.Patients[index].Id || this.SelectedPatientId == this.Patients[index].Id)
        {
          return index;
        }
      }

      return -1;
    }

    private CodeableConcept GeneratedCodeConcept(string Code, string Display)
    {
      Coding CodingItem = new Coding("http://terminology.hl7.org/CodeSystem/allergyintolerance-clinical", Code, Display);

      List<Coding> CodingList = new List<Coding>();
      CodingList.Add(CodingItem);

      CodeableConcept CodeConcept = new CodeableConcept();
      CodeConcept.Coding = CodingList;

      return CodeConcept;
    }

    private Narrative GeneratedText()
    {
      string rootBlock = "<div xmlns=\"http://www.w3.org/1999/xhtml\">";
      string headingBlock = "<p class=\"allergyintolerance_narrative\"><b>Generated Narrative with Details</b></p>";
      string idBlock = $"<p class=\"allergyintolerance_id\"><b>id</b>: {this.AllergyIntolerance.Id}</p>";

      string identifierBlock = $"<p class=\"allergyintolerance_identifier\"><b>identifier</b>: {(this.AllergyIntolerance.Identifier.Count > 0 ? this.AllergyIntolerance.Identifier[0].Value + "(OFFICIAL)" : "")}</p>";

      string clinicalStatusBlock = $"<p class=\"allergyintolerance_clinicalstatus\"><b>clinicalStatus</b>: {this.AllergyIntolerance.ClinicalStatus.Coding[0].Display} <span>(<a href=\"{this.AllergyIntolerance.ClinicalStatus.Coding[0].System}\">AllergyIntolerance Clinical Status Codes</a>#active)</span></p>";

      string verificationStatusBlock = $"<p class=\"allergyintolerance_verificationstatus\"><b>verificationStatus</b>: Confirmed <span> (<a href=\"codesystem-allergyintolerance-verification.html\">AllergyIntolerance Verification Status Codes</a>#confirmed)</span></p>";

      string noteBlock = string.Empty;

      if (this.AllergyIntolerance.Note != null && this.AllergyIntolerance.Note.Count > 0)
      {
        noteBlock = $"<p class=\"allergyintolerance_note\"><b>note</b>: ";
        foreach (var note in this.AllergyIntolerance.Note)
        {
          noteBlock += $"{note.Text.ToString()} ";
        }
        noteBlock += "</p>";
      }

      string endBlock = "</div>";

      return new Narrative()
      {
        Status = Narrative.NarrativeStatus.Generated,
        Div = rootBlock + headingBlock + idBlock + identifierBlock + noteBlock + endBlock
      };
    }

    protected async void SaveAllergyIntolerance()
    {
      this.AllergyIntolerance.Text = this.GeneratedText();

      await OnAllergyIntoleranceSaved.InvokeAsync(this.AllergyIntolerance);
    }
  }
}
