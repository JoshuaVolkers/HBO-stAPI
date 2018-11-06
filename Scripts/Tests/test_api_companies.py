import api_schemas
import requests
import cerberus
import pytest
from cerberus import Validator

def f_test_company(company):
    v = Validator(api_schemas.company_schema)
    correct = v.validate(company)
    assert correct == True

def f_test_base_company(base_company):
    v = Validator(api_schemas.base_company_schema)
    correct = v.validate(base_company)
    assert correct == True

def f_test_max_companies(companies, max):
    correct = True
    if len(companies) > max:
        correct = False

    assert correct == True

def f_test_min_stars(companies, min_stars):
    for i in range(0, len(companies)):
        base_company = companies[i]
        correct = True
        if base_company["averageReviewStars"] < min_stars:
            correct = False

        assert correct == True

def f_test_max_stars(companies, max_stars):
    for i in range(0, len(companies)):
        base_company = companies[i]
        correct = True
        if base_company["averageReviewStars"] > max_stars:
            correct = False

        assert correct == True

def f_test_country_name(companies, country_name):
    for i in range(0, len(companies)):
        base_company = companies[i]
        correct = True
        if base_company["location"]["countryName"] != country_name:
            correct = False

        assert correct == True

def f_test_city_name(companies, city_name):
    for i in range(0, len(companies)):
        base_company = companies[i]
        correct = True
        if base_company["location"]["city"] != city_name:
            correct = False

        assert correct == True

def f_test_location_range(companies, r):
    for i in range(0, len(companies)):
        base_company = companies[i]
        correct = True
        if base_company["location"]["range"] > r:
            correct = False

        assert correct == True

def f_test_major(companies, major_name):
    for i in range(0, len(companies)):
        base_company = companies[i]
        correct = False
        majors = base_company["majors"].split(",")

        for m in range(0, len(majors)):
            if majors[m] == major_name:
                correct = True
                break

        assert correct == True

def f_test_status_code_200(request):
    assert request.status_code == 200



base_url = ''
companies_path = "companies"

# set global base_url variable
@pytest.mark.unit
def test_api_path(apiurl):
    global base_url
    base_url = apiurl

# Test multiple companies (Default)
def test_companies_default():
    companies_request = requests.get(base_url+companies_path)
    f_test_status_code_200(companies_request)
    companies = companies_request.json()
    f_test_max_companies(companies, 5)
    f_test_base_company(companies[0])

# Test multiple companies (detailedCompanies = true)
def test_companies_detailed():
    companies_request = requests.get(base_url+companies_path+"?detailedCompanies=true")
    f_test_status_code_200(companies_request)
    companies = companies_request.json()
    f_test_company(companies[0])

# Test multiple companies (minStars = 2)
def test_companies_minstars():
    companies_request = requests.get(base_url+companies_path+"?minStars=2")
    f_test_status_code_200(companies_request)
    companies = companies_request.json()
    f_test_min_stars(companies, 2)
    
# Test multiple companies (maxStars = 2)
def test_companies_maxstars():
    companies_request = requests.get(base_url+companies_path+"?maxStars=2")
    f_test_status_code_200(companies_request)
    companies = companies_request.json()
    f_test_max_stars(companies, 2)

# Test multiple companies (countryName = Nederland)
def test_companies_countryname():
    companies_request = requests.get(base_url+companies_path+"?countryName=Nederland")
    f_test_status_code_200(companies_request)
    companies = companies_request.json()
    f_test_country_name(companies, "Nederland")


# Test multiple companies (cityName = Haarlem)
def test_companies_cityname():
    companies_request = requests.get(base_url+companies_path+"?cityName=Haarlem")
    f_test_status_code_200(companies_request)
    companies = companies_request.json()
    f_test_city_name(companies, "Haarlem")

# Test multiple companies (cityName = Haarlem, locationRange = 20)
def test_companies_range():
    r = 20
    companies_request = requests.get(base_url+companies_path+"?cityName=Haarlem&locationRange="+str(r))
    f_test_status_code_200(companies_request)
    companies = companies_request.json()
    f_test_location_range(companies, r)

# Test multiple companies (major = 1)
def test_companies_major():
    major = 1
    major_name = "Informatica"
    companies_request = requests.get(base_url+companies_path+"?major="+str(major))
    f_test_status_code_200(companies_request)
    companies = companies_request.json()
    f_test_major(companies, major_name)