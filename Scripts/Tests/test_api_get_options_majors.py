import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator

def f_test_major(major):
    v = Validator(api_schemas.major_schema)
    correct = v.validate(major)
    assert correct == True

def f_test_max_majors(majors, max):
    correct = True
    if len(majors) > max:
        correct = False

    assert correct == True

def f_test_status_code_200(request):
    assert request.status_code == 200



base_url = ''
majors_path = "options/majors?maxCount=5&offset=0"

# set global base_url variable
@pytest.mark.unit
def test_api_path(apiurl):
    global base_url
    base_url = apiurl

# Test multiple majors (Default)
def test_majors_default():
    majors_request = requests.get(base_url+majors_path)
    f_test_status_code_200(majors_request)
    majors = majors_request.json()
    f_test_max_majors(majors, 5)
