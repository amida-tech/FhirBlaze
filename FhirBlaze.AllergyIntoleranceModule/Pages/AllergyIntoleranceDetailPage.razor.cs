using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace FhirBlaze.AllergyIntoleranceModule.Pages
{
  [Authorize]
  public partial class AllergyIntoleranceDetailPage
  {
    [Inject]
    private IFhirService FhirService { get; set; }

    [Parameter]
    public string Id { get; set; }

    public List<Patient> Patients { get; set; }

    public List<Practitioner> Practitioners { get; set; }

    // public ValueSet ValuesetCodes { get; set; }

    private AllergyIntolerance SelectedAllergyIntolerance { get; set; } = new AllergyIntolerance();

    protected override async Task OnParametersSetAsync()
    {
      try
      {
        this.Patients = new List<Patient>(await FhirService.GetPatientsAsync());
        this.Practitioners = new List<Practitioner>(await FhirService.GetPractitionersAsync());
        // this.ValuesetCodes = await FhirService.GetResourceByIdAsync<ValueSet>("AllergyIntolerance-codes");

        if (this.Id != null)
        {
          this.SelectedAllergyIntolerance = await FhirService.GetResourceByIdAsync<AllergyIntolerance>(this.Id);
        }
        else
        {
          this.SelectedAllergyIntolerance = new AllergyIntolerance();
        }
      }
      catch (System.Exception e)
      {
        Console.WriteLine("Error in AllergyIntoleranceDetailPage.razor.cs: " + e.Message + "; Source: " + e.Source);
      }
    }

    private async Task SaveAllergyIntolerance(AllergyIntolerance allergyIntolerance)
    {
      AllergyIntolerance persistedAllergyIntolerance = new AllergyIntolerance();
      try
      {
        if (string.IsNullOrEmpty(allergyIntolerance.Id))
        {
          persistedAllergyIntolerance = await FhirService.CreateAllergyIntolerancesAsync(allergyIntolerance);
        }
        else
        {
          persistedAllergyIntolerance = await FhirService.UpdateAllergyIntoleranceAsync(allergyIntolerance.Id, allergyIntolerance);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Exception: {ex.Message}");
      }

      this.SelectedAllergyIntolerance = persistedAllergyIntolerance;
    }
  }
}
