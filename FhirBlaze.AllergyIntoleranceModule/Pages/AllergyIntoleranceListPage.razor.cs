using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace FhirBlaze.AllergyIntoleranceModule.Pages
{
  [Authorize]
  public partial class AllergyIntoleranceListPage
  {
    private bool _loading = true;

    [Inject]
    private IFhirService FhirService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [CascadingParameter]
    public System.Threading.Tasks.Task<AuthenticationState> AuthTask { get; set; }

    private bool ShowSearch { get; set; } = false;

    public IList<AllergyIntolerance> AllergyIntolerances { get; set; } = new List<AllergyIntolerance>();

    protected override async Task OnInitializedAsync()
    {
      _loading = true;
      AllergyIntolerances = await FhirService.GetAllergyIntolerancesAsync();
      _loading = false;
    }

    private void AddAllergyIntoleranceClicked()
    {
      NavigationManager.NavigateTo("/allergyintolerance");
    }

    private void AllergyIntoleranceSelected(AllergyIntolerance allergyIntolerance)
    {
      NavigationManager.NavigateTo($"/allergyintolerance/{allergyIntolerance.Id}");
    }

    private async Task SearchAllergyIntolerance(IDictionary<string, string> searchParameters)
    {
      try
      {
        _loading = true;
        this.AllergyIntolerances = await FhirService.SearchAllergyIntolerance(searchParameters);
        _loading = false;
      }
      catch (Exception e)
      {
        Console.WriteLine("Error" + e.Message);
      }
    }

  }
}
