import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator

base_url = "https://elbho-api-dev.azurewebsites.net/"
companies_path = "companies"

def f_test_company(company):
    v = Validator(api_schemas.company_schema)
    correct = v.validate(company)
    #print("Company schema test conducted");
    #print()
    #print(v.errors)
    assert correct == True

def f_test_company_id(company, id):
    correct = True
    if company["id"] != id:
        correct = False
    #print("Company id test conducted")

    assert correct == True

def f_test_status_code_200(request):
    #print("Status code test conducted")
    assert request.status_code == 200

# Test single company
def test_company():
    id=1
    company_request = requests.get(base_url+companies_path+"/"+str(id))
    f_test_status_code_200(company_request)
    company = company_request.json()
    f_test_company(company)
    f_test_company_id(company, id)