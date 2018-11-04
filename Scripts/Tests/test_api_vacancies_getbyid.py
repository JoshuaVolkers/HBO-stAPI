import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator

base_url = ""
vacancies_path = "vacancies"

# set global base_url variable
def test_api_path(apiurl):
    global base_url
    base_url = apiurl

def test_vacancy():
    id = 1
    vacancy_request = requests.get(base_url+vacancies_path+"/"+str(id))
    f_test_status_code_200(vacancy_request)
    vacancy = vacancy_request.json()
    v = Validator(api_schemas.vacancy_schema)
    correct = v.validate(vacancy)
    assert correct == True

def f_test_status_code_200(request):
    assert request.status_code == 200
