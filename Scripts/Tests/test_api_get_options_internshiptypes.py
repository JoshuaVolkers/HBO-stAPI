import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator

def f_test_internshipType(internshipType):
    v = Validator(api_schemas.internshipType_schema)
    correct = v.validate(internshipType)
    assert correct == True

def f_test_status_code_200(request):
    assert request.status_code == 200

base_url = ''
internshipTypes_path = "options/internshipTypes"

# set global base_url variable
@pytest.mark.unit
def test_api_path(apiurl):
    global base_url
    base_url = apiurl

# Test multiple internshipTypes (Default)
def test_internshipTypes_default():
    internshipTypes_request = requests.get(base_url+internshipTypes_path)
    f_test_status_code_200(internshipTypes_request)
    internshipTypes = internshipTypes_request.json()
    f_test_internshipType(internshipTypes[0])

