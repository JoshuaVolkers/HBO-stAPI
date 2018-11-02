import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator

def f_test_allowedEmailaddresses(allowedEmailaddress):
    v = Validator(api_schemas.allowedEmailadress_schema)
    correct = v.validate(allowedEmailaddress)
    assert correct == True

def f_test_max_allowedEmailaddresses(allowedEmailaddresses, max):
    correct = True
    if len(allowedEmailaddresses) > max:
        correct = False

    assert correct == True

def f_test_status_code_200(request):
    assert request.status_code == 200



base_url = ''
allowedEmailaddresses_path = "options/allowedemailaddresses?maxCount=5&offset=0"

# set global base_url variable
@pytest.mark.unit
def test_api_path(apiurl):
    global base_url
    base_url = apiurl

# Test multiple allowedEmailaddresses (Default)
def test_allowedEmailaddresses_default():
    allowedEmailaddresses_request = requests.get(base_url+allowedEmailaddresses_path)
    f_test_status_code_200(allowedEmailaddresses_request)
    allowedEmailaddresses = allowedEmailaddresses_request.json()
    f_test_allowedEmailaddresses(allowedEmailaddresses[0])
    f_test_max_allowedEmailaddresses(allowedEmailaddresses, 5)
