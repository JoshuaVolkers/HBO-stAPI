import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator
import json

base_url = ''
users_path = "users"

headers = ''

#Set global API Path
def test_api_path(apiurl):
    global base_url
    base_url = apiurl

#Set global student email    
def test_studentemail(studentemail):
    global studentenemail
    studentenemail = studentemail

#Login and set the bearer token
def test_login_student(studentpass):
    payload = {'emailAddress': studentenemail, 'password': studentpass}
    login_request = requests.post(base_url+users_path+"/login", json=payload)
    f_test_status_code_200(login_request)
    authtoken = login_request.json()
    
    global headers
    headers = {'Authorization': 'Bearer '+authtoken}
    
def test_favorite_vacancies():
    id = 2
    favorite_request = requests.get(base_url+users_path+"/me/favorites", headers=headers)
    f_test_status_code_200(favorite_request)
    vacancies = favorite_request.json()
    correct = False
    if len(vacancies) > 0:
        for i in range(0, len(vacancies)):
            vacancy = vacancies[i]
            f_test_vacancy(vacancy)
            if vacancy["id"] == id:
                correct = True
                
    assert correct == True

def f_test_vacancy(vacancy):
    v = Validator(api_schemas.vacancy_schema)
    correct = v.validate(vacancy)
    assert correct == True
    
def f_test_status_code_200(request):
    assert request.status_code == 200  
    