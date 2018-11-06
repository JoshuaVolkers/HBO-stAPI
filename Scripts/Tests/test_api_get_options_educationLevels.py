import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator

def f_test_educationLevel(educationLevel):
    v = Validator(api_schemas.educationLevel_schema)
    correct = v.validate(educationLevel)
    assert correct == True

def f_test_max_educationLevels(educationLevels, max):
    correct = True
    if len(educationLevels) > max:
        correct = False

    assert correct == True

def f_test_status_code_200(request):
    assert request.status_code == 200



base_url = ''
educationLevels_path = "options/educationLevels?maxCount=5&offset=0"

# set global base_url variable
@pytest.mark.unit
def test_api_path(apiurl):
    global base_url
    base_url = apiurl

# Test multiple educationLevels (Default)
def test_educationLevels_default():
    educationLevels_request = requests.get(base_url+educationLevels_path)
    f_test_status_code_200(educationLevels_request)
    educationLevels = educationLevels_request.json()
    f_test_max_educationLevels(educationLevels, 5)

