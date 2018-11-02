import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator

def f_test_language(language):
    v = Validator(api_schemas.language_schema)
    correct = v.validate(language)
    assert correct == True

def f_test_status_code_200(request):
    assert request.status_code == 200

base_url = ''
languages_path = "options/languages"

# set global base_url variable
@pytest.mark.unit
def test_api_path(apiurl):
    global base_url
    base_url = apiurl

# Test multiple languages (Default)
def test_languages_default():
    languages_request = requests.get(base_url+languages_path)
    f_test_status_code_200(languages_request)
    languages = languages_request.json()
    f_test_language(languages[0])

