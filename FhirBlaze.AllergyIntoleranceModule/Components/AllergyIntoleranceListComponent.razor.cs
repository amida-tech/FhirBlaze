using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace FhirBlaze.AllergyIntoleranceModule.Components
{
  public partial class AllergyIntoleranceListComponent
  {
    [Parameter]
    public EventCallback<AllergyIntolerance> OnAllergyIntoleranceSelected { get; set; }

    [CascadingParameter]
    public IList<AllergyIntolerance> AllergyIntolerances { get; set; } = new List<AllergyIntolerance>();

    private void AllergyIntoleranceSelected(AllergyIntolerance allergyIntolerance)
    {
      OnAllergyIntoleranceSelected.InvokeAsync(allergyIntolerance);
    }
  }
}
