using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts.Dto;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsService
    {
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);
        List<PersonResponse> GetAllPersons();
        PersonResponse? GetPersonByPersonId(Guid? personId);
        List<PersonResponse> GetFilteretPerson(string SearchBy, string? SearchString);
        List<PersonResponse> GetSortedPersons(List<PersonResponse> allpersons, string SortedBy, SortedOrderEnum sortOrder);
        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);
        bool DeletePerson(Guid? PersonID);
    }
}
