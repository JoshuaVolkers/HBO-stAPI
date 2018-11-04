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
    vacancies_request = requests.get(base_url+vacancies_path)
    f_test_status_code_200(vacancies_request)
    vacancies = vacancies_request.json()
    correct = False
    if len(vacancies) > 0:
        v = Validator(api_schemas.vacancy_schema)
        vacancy = vacancies[0]
        correct = v.validate(vacancy)

    assert correct == True


def test_max_vacancies():
    max = 5
    vacancies_request = requests.get(base_url+vacancies_path)
    f_test_status_code_200(vacancies_request)
    vacancies = vacancies_request.json()
    correct = True
    if len(vacancies) > max:
        correct = False

    assert correct == True


def test_education():
    education = "Informatica"
    vacancies_request = requests.get(base_url+vacancies_path+"?educationId=1")
    f_test_status_code_200(vacancies_request)
    vacancies = vacancies_request.json()
    if len(vacancies) > 0:
        for i in range(0, len(vacancies)):
            vacancy = vacancies[i]
            correct = True
            if education not in vacancy["education"]:
                correct = False

            assert correct == True


def test_educational_attainment():
    attainment = "HBO Bachelor"
    vacancies_request = requests.get(base_url+vacancies_path+"?educationalAttainmentId=1")
    f_test_status_code_200(vacancies_request)
    vacancies = vacancies_request.json()
    if len(vacancies) > 0:
        for i in range(0, len(vacancies)):
            vacancy = vacancies[i]
            correct = True
            if attainment not in vacancy["educationalAttainment"]:
                correct = False

            assert correct == True


def test_internshiptype():
    internshiptype = "Meeloopstage"
    vacancies_request = requests.get(base_url+vacancies_path+"?internshipType=Meeloopstage")
    print(vacancies_request)
    f_test_status_code_200(vacancies_request)
    vacancies = vacancies_request.json()

    if len(vacancies) > 0:
        for i in range(0, len(vacancies)):
            vacancy = vacancies[i]
            correct = True
            if internshiptype not in vacancy["internshipType"]:
                correct = False

            assert correct == True


def test_country_name():
    country_name = "Nederland"
    vacancies_request = requests.get(base_url+vacancies_path+"?countryName="+country_name)
    f_test_status_code_200(vacancies_request)
    vacancies = vacancies_request.json()

    if len(vacancies) > 0:
        for i in range(0, len(vacancies)):
            vacancy = vacancies[i]
            correct = True
            if vacancy["location"]["countryName"] != country_name:
                correct = False

            assert correct == True


def test_city_name():
    city_name = "Haarlem"
    vacancies_request = requests.get(base_url+vacancies_path+"?cityName="+city_name)
    f_test_status_code_200(vacancies_request)
    vacancies = vacancies_request.json()

    if len(vacancies) > 0:
        for i in range(0, len(vacancies)):
            vacancy = vacancies[i]
            correct = True
            if vacancy["location"]["city"] != city_name:
                correct = False

            assert correct == True


def test_location_range():
    r = 20
    vacancies_request = requests.get(base_url+vacancies_path+"?cityName=Haarlem&locationRange="+str(r))
    f_test_status_code_200(vacancies_request)
    vacancies = vacancies_request.json()

    if len(vacancies) > 0:
        for i in range(0, len(vacancies)):
            vacancy = vacancies[i]
            correct = True
            if vacancy["location"]["range"] > r:
                correct = False

            assert correct == True


def f_test_status_code_200(request):
    assert request.status_code == 200
