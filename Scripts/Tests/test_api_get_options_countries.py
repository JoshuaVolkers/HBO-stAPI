import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator

def f_test_country(country):
    v = Validator(api_schemas.country_schema)
    correct = v.validate(country)
    assert correct == True

def f_test_status_code_200(request):
    assert request.status_code == 200

base_url = ''
countries_path = "options/countries"

# set global base_url variable
@pytest.mark.unit
def test_api_path(apiurl):
    global base_url
    base_url = apiurl

def test_countries():
    countries_request = requests.get(base_url+countries_path)
    f_test_status_code_200(countries_request)
    countries = countries_request.json()
    f_test_country(countries[0])