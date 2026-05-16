@extends('z_ki-admin.layout.master')
@section('title', 'Project Dashboard')
@section('css')

    <!-- apexcharts css-->
    <link rel="stylesheet" type="text/css" href="{{ asset('assets/vendor/apexcharts/apexcharts.css') }}">

    <!-- slick css -->
    <link href="{{ asset('../assets/vendor/slick/slick.css')}}" rel="stylesheet">
    <link href="{{ asset('../assets/vendor/slick/slick-theme.css')}}" rel="stylesheet">


@endsection
@section('main-content')
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-7 col-xxl-5 ">
                <div class="card overview-details-box b-s-3-primary ">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-6">
                                <div class="d-flex gap-3 align-items-center">
                                                <span class="bg-primary h-60 w-60 d-flex-center flex-column rounded-3">
                                                    <span class="f-w-500">Mon</span>
                                                    <span>20</span>
                                                </span>

                                    <div>
                                        <p class="text-dark f-w-600 txt-ellipsis-1">Task Overview </p>
                                        <div class="chart-card-box d-flex align-items-center">
                                            <div id="taskOverview"></div>
                                            <span class="badge bg-primary b-r-50">
                                                           80
                                                        </span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-6 mt-3 mt-sm-0">
                                <div class="d-flex align-items-center gap-1">
                                    <div class="flex-grow-1">
                                        <p class="text-dark f-w-500 txt-ellipsis-1">Provided Time</p>
                                        <h6 class="mb-0 text-primary">6 Day's</h6>
                                    </div>
                                    <div class="flex-grow-1">
                                        <p class="text-dark f-w-500 txt-ellipsis-1">Working Time</p>
                                        <h6 class="mb-0 text-primary">60M</h6>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card overview-details-box b-s-3-success ">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-6">
                                <div class="d-flex gap-3 align-items-center">
                                                <span class="bg-success h-60 w-60 d-flex-center flex-column rounded-3">
                                                    <span class="f-w-500">Fri</span>
                                                    <span>22</span>
                                                </span>

                                    <div>
                                        <p class="text-dark f-w-600 txt-ellipsis-1">Task Overview </p>
                                        <div class="chart-card-box d-flex align-items-center">
                                            <div id="taskOverview1"></div>
                                            <span class="badge bg-success b-r-50">
                                                           152
                                                        </span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-6 mt-3 mt-sm-0 gap-1">
                                <div class="d-flex align-items-center">
                                    <div class="flex-grow-1">
                                        <p class="text-dark f-w-500 txt-ellipsis-1">Provided Time</p>
                                        <h6 class="mb-0 text-success">8 Day's</h6>
                                    </div>
                                    <div class="flex-grow-1">
                                        <p class="text-dark f-w-500 txt-ellipsis-1">Working Time</p>
                                        <h6 class="mb-0 text-success">40M</h6>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card overview-details-box b-s-3-danger ">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-6">
                                <div class="d-flex gap-3 align-items-center">
                                                <span class="bg-danger h-60 w-60 d-flex-center flex-column rounded-3">
                                                    <span class="f-w-500">Wed</span>
                                                    <span>25</span>
                                                </span>

                                    <div>
                                        <p class="text-dark f-w-600 txt-ellipsis-1">Task Overview </p>
                                        <div class="chart-card-box d-flex align-items-center">
                                            <div id="taskOverview2"></div>
                                            <span class="badge bg-danger b-r-50">
                                                           200
                                                        </span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-6 mt-3 mt-sm-0 gap-1">
                                <div class="d-flex align-items-center">
                                    <div class="flex-grow-1">
                                        <p class="text-dark f-w-500 txt-ellipsis-1">Provided Time</p>
                                        <h6 class="mb-0 text-danger">3 Week</h6>
                                    </div>
                                    <div class="flex-grow-1">
                                        <p class="text-dark f-w-500 txt-ellipsis-1">Working Time</p>
                                        <h6 class="mb-0 text-danger">80H</h6>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-5 col-xxl-3">
                <div class="card overflow-hidden equal-card">
                    <div class="card-body p-0">
                        <div class="meeting-call-box bg-gradient-mode">
                            <img alt="img"
                                 class="img-fluid position-relative z-1"
                                 src="{{asset('../assets/images/dashboard/project/meeting-avtar.png')}}">
                            <img alt="img"
                                 class="img-fluid bg-vector-img"
                                 src="{{asset('../assets/images/dashboard/project/bg-round.png')}}">
                            <img alt="img"
                                 class="img-fluid bg-vector-img1"
                                 src="{{asset('../assets/images/dashboard/project/bg-round2.png')}}">

                            <div class="meeting-details-box d-flex align-items-center">
                                <div class="h-40 w-40 d-flex-center b-r-50 overflow-hidden bg-dark flex-shrink-0">
                                    <img alt="image" class="img-fluid"
                                         src="{{asset('../assets/images/avatar/2.png')}}">
                                </div>
                                <div class="flex-grow-1 ps-2 text-start">
                                    <div class="fw-medium txt-ellipsis-1"> Bette Hagenes</div>
                                    <div class="text-muted f-s-12 txt-ellipsis-1">Wed Developer</div>
                                </div>
                                <button class="btn btn-success btn-sm">Join</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-7 col-lg-4">

                <div class="card equal-card project-data-container">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center">
                            <h6 class="mb-0">Project </h6>

                            <form class="app-form flex-shrink-0">
                                <select aria-label="Default select example"
                                        class="form-select custom-form-select ">
                                    <option selected="">Filter</option>
                                    <option value="1">Fashion</option>
                                    <option value="2">Books</option>
                                    <option value="3">Sports</option>
                                    <option value="4">Fitness</option>
                                </select>
                            </form>
                        </div>
                        <div class="row project-row mt-4">
                            <div class="col-sm-4">
                                <div class="project-status-card bg-primary text-center w-100 rounded p-3 shadow">
                                             <span class="bg-white h-45 w-45 d-flex-center b-r-50 status-icon">
                                                <i class="ti ti-clock-hour-5 f-s-20 text-primary"></i>
                                             </span>
                                    <p class="text-white mb-0 txt-ellipsis-1">Running</p>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="project-status-card bg-success text-center w-100 rounded p-3 shadow">
                                            <span class="bg-white h-45 w-45 d-flex-center b-r-50 status-icon">
                                                <i class="ti ti-circle-check f-s-20 text-success"></i>
                                             </span>
                                    <p class="text-white mb-0 txt-ellipsis-1">Completed</p>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="project-status-card bg-danger text-center w-100 rounded p-3 shadow">
                                            <span class="bg-white h-45 w-45 d-flex-center b-r-50 status-icon">
                                                <i class="ti ti-refresh f-s-20 text-danger"></i>
                                             </span>
                                    <p class="text-white mb-0 txt-ellipsis-1">Pending</p>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-4">
                            <div class="col-sm-6">
                                <ul class="">
                                    <li class="d-flex align-items-center justify-content-between">
                                        <div class="bg-danger-200 d-flex-center p-1 w-30 h-30 b-r-8 flex-shrink-0">
                                            <img alt="avtaar" class="img-fluid" src="{{asset('../assets/images/icons/language/logo1.png')}}">
                                        </div>
                                        <div class="ms-2 flex-grow-1">
                                            <p class="text-dark-800 mb-0 f-w-500 f-s-15 txt-ellipsis-1">New Task Assigned</p>
                                        </div>
                                    </li>
                                    <li class="d-flex align-items-center justify-content-between mt-3">
                                        <div class="bg-success-200 d-flex-center p-1 w-30 h-30 b-r-8 flex-shrink-0">
                                            <img alt="avatar" class="img-fluid" src="{{asset('../assets/images/icons/language/logo5.png')}}">
                                        </div>
                                        <div class="ms-2 flex-grow-1">
                                            <p class="text-dark-800 mb-0 f-w-500 f-s-15 txt-ellipsis-1">Database Migration</p>
                                        </div>
                                    </li>
                                    <li class="d-flex align-items-center justify-content-between mt-3">
                                        <div class="bg-info-200 d-flex-center p-1 w-30 h-30 b-r-8 flex-shrink-0">
                                            <img alt="avatar" class="img-fluid" src="{{asset('../assets/images/icons/language/logo6.png')}}">
                                        </div>
                                        <div class="ms-2 flex-grow-1">
                                            <p class="text-dark-800 mb-0 f-w-500 f-s-15 txt-ellipsis-1">New Task Assigned</p>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                            <div class="col-sm-6">
                                <ul class="">
                                    <li class="d-flex align-items-center justify-content-between">
                                        <div class="bg-primary-200 d-flex-center p-1 w-30 h-30 b-r-8 flex-shrink-0">
                                            <img alt="avatar" class="img-fluid" src="{{asset('../assets/images/icons/language/logo4.png')}}">
                                        </div>
                                        <div class="ms-2 flex-grow-1">
                                            <p class="text-dark-800 mb-0 f-w-500 f-s-15 txt-ellipsis-1">API Development
                                                Phase</p>
                                        </div>
                                    </li>
                                    <li class="d-flex align-items-center justify-content-between mt-3">
                                        <div class="bg-danger-200 d-flex-center p-1 w-30 h-30 b-r-8 flex-shrink-0">
                                            <img alt="avatar" class="img-fluid" src="{{asset('../assets/images/icons/language/logo3.png')}}">
                                        </div>
                                        <div class="ms-2 flex-grow-1">
                                            <p class="text-dark-800 mb-0 f-w-500 f-s-15 txt-ellipsis-1">UI/UX Design Update</p>
                                        </div>
                                    </li>
                                    <li class="d-flex align-items-center justify-content-between mt-3">
                                        <div class="bg-info-200 d-flex-center p-1 w-30 h-30 b-r-8 flex-shrink-0">
                                            <img alt="avatar" class="img-fluid" src="{{asset('../assets/images/icons/language/logo2.png')}}">
                                        </div>
                                        <div class="ms-2 flex-grow-1">
                                            <p class="text-dark-800 mb-0 f-w-500 f-s-15 txt-ellipsis-1">New Task Assigned</p>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </div>


                    </div>
                </div>
            </div>

            <div class="col-sm-6 col-md-5 col-lg-4 col-xxl-3">
                <div class="card">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center">
                            <h6 class="mb-0 txt-ellipsis-1">Yearly Earning</h6>

                            <form class="app-form ms-2">
                                <select aria-label="Default select example"
                                        class="form-select custom-form-select">
                                    <option selected="">Jan</option>
                                    <option value="1">Feb</option>
                                    <option value="2">Mar</option>
                                    <option value="3">...</option>
                                    <option value="4">Dec</option>
                                </select>
                            </form>
                        </div>
                        <div id="audienceChart"></div>
                        <ul class="">
                            <li class="py-1 d-flex align-items-center justify-content-between">
                                <p class="mb-0 txt-ellipsis-1">
                                    <i class="ti ti-circle-filled text-primary f-s-10"></i> 19-20 years
                                </p>
                                <p class="text-secondary txt-ellipsis-1 mb-0 flex-grow-1 mx-2"> ------------------------ </p>
                                <span>68%</span>
                            </li>
                            <li class="py-1 d-flex align-items-center justify-content-between">
                                <p class="mb-0 txt-ellipsis-1"><i class="ti ti-circle-filled text-primary-800 f-s-10"></i> 20-21 years</p>
                                <p class="text-secondary txt-ellipsis-1 mb-0 flex-grow-1 mx-2"> ------------------------ </p>
                                <span>58%</span>
                            </li>
                            <li class="py-1 d-flex align-items-center justify-content-between">
                                <p class="mb-0 txt-ellipsis-1"><i class="ti ti-circle-filled text-primary-600 f-s-10"></i> 21-22 years</p>
                                <p class="text-secondary txt-ellipsis-1 mb-0 flex-grow-1 mx-2"> ------------------------ </p>
                                <span>78%</span>
                            </li>
                            <li class="py-1 d-flex align-items-center justify-content-between">
                                <p class="mb-0 txt-ellipsis-1"><i class="ti ti-circle-filled text-primary-400 f-s-10"></i> 22-23 years</p>
                                <p class="text-secondary txt-ellipsis-1 mb-0 flex-grow-1 mx-2"> ----------------------- </p>
                                <span>88%</span>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>

            <div class="col-sm-6 col-lg-4 col-xxl-3">

                <div class="card updated-card equal-card">
                    <div class="card-body text-center">

                        <div class="updates-box-slider app-arrow">
                            <div>
                                <div class="bg-light-primary b-r-12 ">
                                    <img alt="img"
                                         class="img-fluid d-block m-auto"
                                         src="{{asset('../assets/images/dashboard/ecommerce-dashboard/01.png')}}">
                                </div>
                                <div class="mt-3">
                                    <p class="f-s-18 f-w-500 txt-ellipsis-2">Improve workflow efficiency
                                        with expert tips & tools!</p>
                                    <button class="btn btn-primary mt-2">Start Now</button>
                                </div>
                            </div>
                            <div>
                                <div class="bg-light-primary b-r-12 ">
                                    <img alt="img"
                                         class="img-fluid d-block m-auto"
                                         src="{{asset('../assets/images/dashboard/ecommerce-dashboard/01.png')}}">
                                </div>
                                <div class="mt-3">
                                    <p class="f-s-18 f-w-500 txt-ellipsis-2">Track your budget, earnings,
                                        and expenses in real time.</p>
                                    <button class="btn btn-primary mt-2">Start Now</button>
                                </div>
                            </div>
                            <div>
                                <div class="bg-light-primary b-r-12 d-block m-auto">
                                    <img alt="img"
                                         class="img-fluid d-block m-auto"
                                         src="{{asset('../assets/images/dashboard/ecommerce-dashboard/01.png')}}">
                                </div>
                                <div class="mt-3">
                                    <p class="f-s-18 f-w-500 txt-ellipsis-2">Boost productivity with smart
                                        project strategies! 🚀</p>
                                    <button class="btn btn-primary my-2">Start Now</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-sm-6 col-lg-4 col-xxl-3">
                <div class="card equal-card">
                    <div class="card-body">
                        <ul class="nav nav-tabs tab-primary bg-primary p-1 rounded updates-tab" id="bg"
                            role="tablist">
                            <li class="nav-item" role="presentation">
                                <button aria-controls="meetingDtaTabsPane" aria-selected="true"
                                        class="nav-link active"
                                        data-bs-target="#meetingDtaTabsPane" data-bs-toggle="tab"
                                        id="meetingDtaTabs"
                                        role="tab"
                                        type="button"> Meetings
                                </button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button aria-controls="NotesDataTabPane" aria-selected="false"
                                        class="nav-link"
                                        data-bs-target="#NotesDataTabPane" data-bs-toggle="tab"
                                        id="NotesDataTab"
                                        role="tab"
                                        type="button"> Notes
                                </button>
                            </li>
                        </ul>
                        <div class="tab-content update-tab-content">
                            <div aria-labelledby="meetingDtaTabs" class="tab-pane fade show active"
                                 id="meetingDtaTabsPane"
                                 role="tabpanel" tabindex="0">
                                <ul class="form-selectgroup">
                                    <li class="select-item">
                                        <input class="form-check-input task-check w-25 h-25"
                                               id="inlineCheckbox1"
                                               type="checkbox" value="option1">
                                        <label class="form-check-label" for="inlineCheckbox1">
                                            <span class="d-flex align-items-center">
                                                <span class="ms-3">
                                                    <span class="fs-6 client-name txt-ellipsis-1">Mark Moen</span>
                                                    <span class="d-block text-secondary">Website Redesign Briefing</span>
                                                </span>
                                            </span>
                                        </label>
                                    </li>
                                    <li class="select-item">
                                        <input class="form-check-input task-check w-25 h-25"
                                               id="inlineCheckbox2"
                                               type="checkbox" value="option2">
                                        <label class="form-check-label" for="inlineCheckbox2">
                                            <span class="d-flex align-items-center">
                                                <span class="ms-3">
                                                    <span class="fs-6 client-name txt-ellipsis-1">Johan Moen</span>
                                                    <span class="d-block text-secondary">CRM Integration Planning</span>
                                                </span>
                                            </span>
                                        </label>
                                    </li>
                                    <li class="select-item">
                                        <input class="form-check-input task-check w-25 h-25"
                                               id="inlineCheckbox3"
                                               type="checkbox" value="option2">
                                        <label class="form-check-label" for="inlineCheckbox3">
                                            <span class="d-flex align-items-center">
                                                <span class="ms-3">
                                                    <span class="fs-6 client-name txt-ellipsis-1">Carlos Ramirez</span>
                                                    <span class="d-block text-secondary">Brand Audit Presentation</span>
                                                </span>
                                            </span>
                                        </label>
                                    </li>
                                    <li class="select-item">
                                        <input class="form-check-input task-check w-25 h-25"
                                               id="inlineCheckbox3"
                                               type="checkbox" value="option2">
                                        <label class="form-check-label" for="inlineCheckbox3">
                                            <span class="d-flex align-items-center">
                                                <span class="ms-3">
                                                    <span class="fs-6 client-name txt-ellipsis-1">Stellar Finances</span>
                                                    <span class="d-block text-secondary">Performance Review </span>
                                                </span>
                                            </span>
                                        </label>
                                    </li>
                                </ul>
                                <button class="btn btn-primary w-100" type="button">Show More</button>
                            </div>
                            <div aria-labelledby="NotesDataTab" class="tab-pane fade" id="NotesDataTabPane"
                                 role="tabpanel" tabindex="1">
                                <div class="w-100 h-250 text-center no-data mt-5">
                                    <img src="{{asset('../assets/images/dashboard/project/no-data.png')}}" class="img-fluid" alt=""/>
                                    <h6 class="f-w-500 text-primary mt-4">No Data Found</h6>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-sm-6 col-md-5 col-lg-4 col-xxl-3 ">
                <div class="card bg-primary">
                    <div class="card-body">
                        <div class="d-flex align-items-center justify-content-between">
                            <h5 class="text-white">Profit Overview</h5>

                        </div>
                        <div>
                            <div id="profitOverview"></div>
                        </div>
                    </div>
                </div>
                <div class="alert  alert-dismissible project-alert" role="alert">
                    <div class="d-flex justify-content-between">
                        <p class="mb-0 z-1 txt-ellipsis-2">
                            🚀 Welcome! Keep track of your projects efficiently.
                        </p>
                        <button aria-label="Close" class="btn-close " data-bs-dismiss="alert"
                                type="button"></button>
                    </div>
                    <div class="progress-bar bg-primary"></div>
                </div>
            </div>

            <div class="col-lg-4 col-xxl-2 order-1-lg">
                <div class="row">
                    <div class="col-sm-6 col-lg-12">
                        <div class="card project-profit-card">
                            <div class="card-body">
                                <div class="profit-arrow">
                             <span class="bg-white text-primary h-45 w-45 d-flex-center">
                                  <i class="ph-bold  ph-arrow-up-right f-s-18"></i>
                             </span>
                                </div>
                                <span class="bg-primary h-45 w-45 d-flex-center b-r-50">
                         <i class="ph-bold  ph-chart-line-up f-s-24"></i>
                        </span>
                                <div class="mt-3">
                                    <h4 class="text-dark">22.2K+</h4>
                                    <p class="f-w-500 mb-0 txt-ellipsis-1">Total profit Progress</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-lg-12">
                        <div class="card bg-primary profit-card-2">
                            <div class="card-body">
                                <i class="ph-duotone  ph-calendar-check icon-bg"></i>
                                <span class="bg-white h-50 w-50 d-flex-center b-r-50">
                          <i class="ph-duotone ph-calendar-check text-primary f-s-24"></i>
                        </span>
                                <div class="mt-3">
                                    <h4 class="text-white">15+</h4>
                                    <p class="f-w-500 mb-0 txt-ellipsis-1">Active Projects</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

            <div class="col-md-7 col-lg-4">
                <div class="header-box">
                    <h5>Online Candidates</h5>
                </div>
                <div class="card">
                    <div class="card-body px-2 pt-2 equal-card">
                        <div class="table-responsive app-scroll">
                            <table class="table table-bottom-border align-middle mb-0">
                                <tbody>

                                <tr class="border-0">
                                    <td>
                                        <div class="d-flex align-items-center">
                                            <div class="h-40 w-40 d-flex-center b-r-12 overflow-hidden bg-danger-600">
                                                <img alt="image" class="img-fluid"
                                                     src="{{asset('../assets/images/avatar/3.png')}}">
                                            </div>
                                            <div class="flex-grow-1 ps-2">
                                                <div class="fw-medium txt-ellipsis-1">Savannah Nguyen</div>
                                                <div class="text-muted f-s-12 txt-ellipsis-1">Project Manager</div>
                                            </div>
                                        </div>
                                    </td>
                                    <td><span class="badge text-light-primary f-s-12 f-w-700">350</span>
                                    </td>
                                    <td>
                                        <div class="form-check form-switch">
                                            <label class="form-check-label f-w-500"
                                                   for="tableCheck">Remove</label>
                                            <input class="form-check-input w-25 form-check-primary"
                                                   id="tableCheck" type="checkbox">
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <div class="d-flex align-items-center">
                                            <div class="h-40 w-40 d-flex-center b-r-12 overflow-hidden bg-info-600">
                                                <img alt="image" class="img-fluid"
                                                     src="{{asset('../assets/images/avatar/1.png')}}">
                                            </div>
                                            <div class="flex-grow-1 ps-2">
                                                <div class="fw-medium txt-ellipsis-1">Bette Hagenes</div>
                                                <div class="text-muted f-s-12 txt-ellipsis-1">Web Designer</div>
                                            </div>
                                        </div>
                                    </td>
                                    <td><span class="badge text-light-primary f-s-12 f-w-700">320</span>
                                    </td>
                                    <td>
                                        <div class="form-check form-switch">
                                            <label class="form-check-label f-w-500"
                                                   for="tableCheck1">Add</label>
                                            <input checked
                                                   class="form-check-input w-25 form-check-primary"
                                                   id="tableCheck1" type="checkbox">
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <div class="d-flex align-items-center">
                                            <div class="h-40 w-40 d-flex-center b-r-12 overflow-hidden bg-warning-600">
                                                <img alt="image" class="img-fluid"
                                                     src="{{asset('../assets/images/avatar/2.png')}}">
                                            </div>
                                            <div class="flex-grow-1 ps-2">
                                                <div class="fw-medium txt-ellipsis-1">Esther Howard</div>
                                                <div class="text-muted f-s-12 txt-ellipsis-1">Software Engineer</div>
                                            </div>
                                        </div>
                                    </td>
                                    <td><span class="badge text-light-primary f-s-12 f-w-700">280</span>
                                    </td>
                                    <td>
                                        <div class="form-check form-switch">
                                            <label class="form-check-label f-w-500"
                                                   for="tableCheck3">Remove</label>
                                            <input class="form-check-input w-25 form-check-primary"
                                                   id="tableCheck3" type="checkbox">
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="pb-0 text-nowrap">
                                        Add New Candidates
                                    </td>
                                </tr>

                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-sm-6 col-lg-4 col-xxl-3">
                <div class="header-box d-flex justify-content-between align-items-center">
                    <h5>Client List</h5>
                </div>
                <div class="card">
                    <div class="card-body">
                        <ul class="nav nav-tabs app-tabs-primary border-0 flex-nowrap overflow-auto" id="Outline" role="tablist">
                            <li class="nav-item" role="presentation">
                                <button class="nav-link active" id="clientTab" data-bs-toggle="tab"
                                        data-bs-target="#clientTabPane" type="button" role="tab" aria-controls="clientTabPane"
                                        aria-selected="true">Client</button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link" id="activeTab" data-bs-toggle="tab"
                                        data-bs-target="#activeTabPane" type="button" role="tab" aria-controls="activeTabPane"
                                        aria-selected="false">Active</button>
                            </li>
                            <li class="nav-item" role="presentation">
                                <button class="nav-link" id="orderTab" data-bs-toggle="tab" data-bs-target="#orderTabPane"
                                        type="button" role="tab" aria-controls="orderTabPane" aria-selected="false">Deactivate</button>
                            </li>
                        </ul>
                        <ul class="box-list client-list">
                            <li class="d-flex align-items-center justify-content-between">
                                <div class="h-45 w-45 d-flex-center b-r-12 overflow-hidden bg-success-500 flex-shrink-0">
                                    <img alt="task" class="img-fluid" src="{{asset('../assets/images/avatar/6.png')}}">
                                </div>
                                <div class="ms-2 flex-grow-1">
                                    <p class="mb-0 f-w-500 f-s-16 txt-ellipsis-1">rayyan-colin</p>
                                    <p class="mb-0 f-s-12 txt-ellipsis-1">1240 points</p>
                                </div>
                                <span class="h-30 w-30 d-flex-center b-r-50">
                                         <i class="ti ti-chevron-right text-primary f-s-20"></i>
                                    </span>
                            </li>
                            <li class="d-flex align-items-center justify-content-between">
                                <div class="h-45 w-45 d-flex-center b-r-12 overflow-hidden bg-warning-500 flex-shrink-0">
                                    <img alt="task" class="img-fluid" src="{{asset('../assets/images/avatar/2.png')}}">
                                </div>
                                <div class="ms-2 flex-grow-1">
                                    <p class="mb-0 f-w-500 f-s-16 txt-ellipsis-1">rayyan-colin</p>
                                    <p class="mb-0 f-s-12 txt-ellipsis-1">1240 points</p>
                                </div>
                                <span class="h-35 w-35 d-flex-center b-r-50">
                                         <i class="ti ti-chevron-right text-primary f-s-20"></i>
                                    </span>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>

            <div class="col-sm-6 col-lg-4 col-xxl-3">
                <ul class="box-list">
                    <li class=" d-flex justify-content-between align-items-center mt-3">
                        <h5 class="mb-0 txt-ellipsis-1 flex-grow-1"> Notifications</h5>
                        <span class="badge bg-primary">3 New</span>
                    </li>
                    <li class="d-flex align-items-center justify-content-between">
                        <div class="h-40 w-40 d-flex-center b-r-12 overflow-hidden bg-primary text-white flex-shrink-0">
                            TN
                        </div>
                        <div class="ms-2 flex-grow-1">
                            <p class="mb-0 f-w-500 f-s-18 txt-ellipsis-1">New Task Assigned</p>
                            <p class="mb-0 f-s-12 txt-ellipsis-1">5 min ago</p>
                        </div>
                        <a href="/ki-admin/chat" target="_blank" class="text-light-success h-45 w-45 d-flex-center b-r-50">
                                         <i class="ti ti-message f-s-20"></i>
                                    </a>
                    </li>
                    <li class="d-flex align-items-center justify-content-between">
                        <div class="h-40 w-40 d-flex-center b-r-12 overflow-hidden bg-success-500 flex-shrink-0">
                            <img alt="task" class="img-fluid" src="{{asset('../assets/images/avatar/2.png')}}">
                        </div>
                        <div class="ms-2 flex-grow-1">
                            <p class="mb-0 f-w-500 f-s-18 txt-ellipsis-1">Task #204 Completed</p>
                            <p class="mb-0 f-s-12 txt-ellipsis-1">10 min ago</p>
                        </div>
                        <a href="/ki-admin/chat" target="_blank" class="text-light-success h-45 w-45 d-flex-center b-r-50">
                                         <i class="ti ti-message f-s-20"></i>
                                    </a>
                    </li>
                    <li class="d-flex align-items-center justify-content-between">
                        <div class="h-40 w-40 d-flex-center b-r-12 overflow-hidden bg-danger-500 flex-shrink-0">
                            <img alt="task" class="img-fluid" src="{{asset('../assets/images/avatar/5.png')}}">
                        </div>
                        <div class="ms-2 flex-grow-1">
                            <p class="mb-0 f-w-500 f-s-18 txt-ellipsis-1">Task #198 Overdue</p>
                            <p class="mb-0 f-s-12 txt-ellipsis-1">20 min ago</p>
                        </div>
                        <a href="/ki-admin/chat" target="_blank" class="text-light-success h-45 w-45 d-flex-center b-r-50">
                                         <i class="ti ti-message f-s-20"></i>
                                    </a>
                    </li>

                </ul>
            </div>

        </div>
    </div>
@endsection

@section('script')

    <!-- slick-file -->
    <script src="{{asset('assets/vendor/slick/slick.min.js')}}"></script>


    <!-- apexcharts js-->
    <script src="{{asset('assets/vendor/apexcharts/apexcharts.min.js')}}"></script>

    <!-- Project js-->
    <script src="{{asset('assets/js/project_dashboard.js')}}"></script>

@endsection
