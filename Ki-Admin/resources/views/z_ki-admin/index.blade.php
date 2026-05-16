@extends('z_ki-admin.layout.master')
@section('title', 'Ecommerce Dashboard')
@section('css')

    <!-- apexcharts css -->
    <link rel="stylesheet" type="text/css" href="{{ asset('assets/vendor/apexcharts/apexcharts.css') }}">

    <!-- glight css -->
    <link rel="stylesheet" href="{{asset('assets/vendor/glightbox/glightbox.min.css')}}">

@endsection
@section('main-content')
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-5 col-xxl-3 eshop-cards-container">
                <div class="row">
                    <div class="col-6 col-md-3 col-lg-6">
                        <div class="card">
                            <span class="bg-primary h-50 w-50 d-flex-center rounded-circle m-auto eshop-icon-box">
                                    <i class="ph  ph-currency-circle-dollar f-s-24"></i>
                            </span>


                            <div class="card-body eshop-cards">
                                <span class="ripple-effect"></span>
                                <div class="overflow-hidden">
                                    <h3 class="text-primary mb-0">1.2M</h3>
                                    <p class="mg-b-35 f-w-600 text-dark-800 txt-ellipsis-1">Total Sales</p>
                                    <span class="badge bg-light-primary">View Report</span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-md-3 col-lg-6">
                        <div class="card">
                              <span class="bg-danger h-50 w-50 d-flex-center rounded-circle m-auto eshop-icon-box">
                                                <i class="ph ph-x-circle f-s-24"></i>
                              </span>
                            <div class="card-body eshop-cards">
                                <span class="ripple-effect"></span>
                                <h3 class="text-danger mb-0">125</h3>
                                <p class="mg-b-35 f-w-600 text-dark-800 txt-ellipsis-1">Canceled Orders</p>
                                <span class="badge bg-light-danger">Refunded</span>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-md-3 col-lg-6">
                        <div class="card">
                            <span class="bg-warning h-50 w-50 d-flex-center rounded-circle m-auto eshop-icon-box">
                                <i class="ph-duotone  ph-certificate f-s-24"></i>
                            </span>
                            <div class="card-body eshop-cards">
                                <span class="ripple-effect"></span>
                                <h3 class="text-warning mb-0 txt-ellipsis-1"> 95% </h3>
                                <p class="mg-b-35 f-w-600 text-dark-800 txt-ellipsis-1">Top Product</p>
                                <span class="badge bg-light-dark">watch X200</span>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-md-3 col-lg-6">
                        <div class="card">
                             <span class="bg-success h-50 w-50 d-flex-center rounded-circle m-auto eshop-icon-box">
                                 <i class="ph-duotone  ph-user-circle-plus f-s-24"></i>
                             </span>
                            <div class="card-body eshop-cards">
                                <span class="ripple-effect"></span>
                                <h3 class="text-success mb-0">8.5k</h3>
                                <p class="mg-b-35 f-w-600 text-dark-800 txt-ellipsis-1">New Customers</p>
                                <span class="badge bg-light-success">Active</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-7 col-xxl-6">
                <div class="card">
                    <div class="card-body">
                        <div class="row align-items-center">
                            <div class="col-sm-6 position-relative">
                                <div class="location-container">
                                    <div class="glass-effect-box position-absolute bottom-0">
                                        <div class="d-flex align-items-center flex-wrap">
                                            <div class="w-65 h-65 overflow-hidden position-absolute top-0 start-0 d-flex-center">
                                                <img alt="image"
                                                     class="img-fluid"
                                                     src="{{asset('../assets/images/dashboard/ecommerce-dashboard/order1.gif')}}">
                                            </div>
                                            <div class="flex-grow-1 ps-5">
                                                <p class="fw-medium mb-0"> OrderId</p>
                                                <h6 class="text-primary mb-0 txt-ellipsis-1 ">r72qU2892</h6>
                                            </div>
                                            <span class="badge text-bg-primary f-s-10 ms-2">• On The Way</span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-6 cart-side-card mt-3 mt-sm-0">
                                <div class="mb-2">
                                    <h6 class="text-dark mb-0 txt-ellipsis-1">Delivery Date : 20 Feb
                                        2025 </h6>
                                </div>
                                <div class="table-responsive">
                                    <table class="table cart-side-table mb-0">
                                        <tbody>
                                        <tr class="total-price">
                                            <th>Sub Total :</th>
                                            <th class="text-end">
                                                <span id="cart-sub">$359.96</span>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <p class="mb-0 txt-ellipsis-1">Discount:</p>
                                            </td>
                                            <td class="text-end text-success">-$53.00</td>
                                        </tr>
                                        <tr>
                                            <td><p class="mb-0 txt-ellipsis-1">Estimated Tax (12.5%) :</p>
                                            </td>
                                            <td class="text-end text-danger">$44.99</td>
                                        </tr>
                                        <tr class="total-price">
                                            <th><p class="mb-0 txt-ellipsis-1">Total (USD) :</p></th>
                                            <th class="text-end">
                                                                 <span id="cart-total">
                                                                   $415.96
                                                                 </span>
                                            </th>
                                        </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <p class="f-w-500 mb-0 txt-ellipsis-1">  Support: <a href="/ki-admin/email" class="link-primary text-d-underline ">https://support.aicom.com/order?r72qU2892</a> </p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card">
                    <div class="card-body">
                        <ul class="app-side-timeline shipping-timeline">
                            <li class="side-timeline-section w-100 right-side complete-step">
                                <div class="side-timeline-icon">
                                                        <span class="bg-primary h-35 w-35 d-flex-center b-r-50">
                                                               <i class="ph-fill  ph-shopping-bag-open f-s-18"></i>
                                                        </span>
                                </div>
                                <div class="timeline-content p-0">
                                    <div>
                                        <h6 class="f-s-15 mb-2 txt-ellipsis-1">Order Placed</h6>
                                        <p class="mb-0  text-dark-800 f-w-400">Feb 14/25 <span
                                                class="text-primary f-w-500">10:15 AM</span></p>
                                    </div>
                                </div>
                            </li>

                            <li class="side-timeline-section w-100 right-side complete-step">
                                <div class="side-timeline-icon">
                                                <span class="bg-primary h-35 w-35 d-flex-center b-r-50">
                                                    <i class="ph-fill  ph-seal-check f-s-18"></i>
                                                </span>
                                </div>
                                <div class="timeline-content p-0">
                                    <div>
                                        <h6 class="f-s-15 mb-2 txt-ellipsis-1">Order Confirmed</h6>
                                        <p class="mb-0  text-dark-800 f-w-400">Feb 14/25 <span
                                                class="text-primary f-w-500">10:30 AM</span></p>
                                    </div>
                                </div>
                            </li>


                            <li class="side-timeline-section w-100 right-side">
                                <div class="side-timeline-icon">
                                                        <span class="bg-light-secondary text-dark-400 h-35 w-35 d-flex-center b-r-50">
                                                            <i class="ph-fill  ph-truck f-s-18"></i>
                                                        </span>
                                </div>
                                <div class="timeline-content p-0">
                                    <div>
                                        <h6 class="f-s-15 mb-2 txt-ellipsis-1">Shipped via Courier</h6>
                                        <p class="mb-0  text-dark-800 f-w-400">Feb 15/25 <span
                                                class="text-primary f-w-500">9:00 AM</span></p>
                                    </div>
                                </div>
                            </li>

                            <li class="side-timeline-section w-100 right-side">
                                <div class="side-timeline-icon">
                                                        <span class="bg-light-secondary text-dark-400 h-35 w-35 d-flex-center b-r-50">
                                                            <i class="ph-fill  ph-house f-s-18"></i>
                                                        </span>
                                </div>
                                <div class="timeline-content p-0">
                                    <div>
                                        <h6 class="f-s-15 mb-2 txt-ellipsis-1">Delivered</h6>
                                        <p class="mb-0  text-dark-800 f-w-400">Feb 18/25 <span
                                                class="text-primary f-w-500">3:45 PM</span></p>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>

            <div class="col-sm-7 col-lg-4 col-xxl-3 ">
                <ul class="notifications-list box-list mb-4 ">
                    <li class="d-flex align-items-center justify-content-between gap-3 b-s-4-primary">
                        <div>
                            <h6 class="txt-ellipsis-1 mb-0"> Order Notifications List</h6>
                            <p class="text-secondary mb-0">Last 2H Updates</p>
                        </div>
                        <div class="h-45 w-45 d-flex-center rounded-circle flex-shrink-0 bg-light-primary position-relative">
                            <i class="ph ph-bell-ringing f-s-20"></i>
                            <span class="position-absolute translate-middle badge rounded-pill bg-danger badge-notification">4</span>
                        </div>
                    </li>
                    <li class="d-flex align-items-center">
                        <div class="h-45 w-45 d-flex-center b-r-12 overflow-hidden flex-shrink-0 bg-primary">
                            OR
                        </div>
                        <div class="ms-3">
                            <p class="mb-0 f-w-500 f-s-16 txt-ellipsis-1">New Order Received</p>
                            <p class="mb-0 text-secondary">5 min ago</p>
                        </div>
                    </li>
                    <li class="d-flex align-items-center">
                        <div class="h-45 w-45 d-flex-center b-r-12 overflow-hidden flex-shrink-0 bg-success-400">
                            <img alt="order" class="img-fluid" src="{{asset('../assets/images/avatar/2.png')}}">
                        </div>
                        <div class="ms-3">
                            <p class="mb-0 f-w-500 f-s-16 txt-ellipsis-1">Order #1024 has been shipped</p>
                            <p class="mb-0 text-secondary">10 min ago</p>
                        </div>
                    </li>
                    <li class="d-flex align-items-center">
                        <div class="h-45 w-45 d-flex-center b-r-12 overflow-hidden flex-shrink-0 bg-danger-400">
                            <img alt="order" class="img-fluid" src="{{asset('../assets/images/avatar/5.png')}}">
                        </div>
                        <div class="ms-3">
                            <p class="mb-0 f-w-500 f-s-16 txt-ellipsis-1">Order #1022 has been canceled</p>
                            <p class="mb-0 text-secondary">20 min ago</p>
                        </div>
                    </li>
                    <li class="d-flex align-items-center">
                        <div class="h-45 w-45 d-flex-center b-r-12 overflow-hidden flex-shrink-0 bg-danger-400">
                            <img alt="order" class="img-fluid" src="{{asset('../assets/images/avatar/5.png')}}">
                        </div>
                        <div class="ms-3">
                            <p class="mb-0 f-w-500 f-s-16 txt-ellipsis-1">Order #1022 has been canceled</p>
                            <p class="mb-0 text-secondary">20 min ago</p>
                        </div>
                    </li>
                </ul>
            </div>

            <div class="col-sm-5 col-lg-4 col-xxl-3 ">
                <div class="card">
                    <div class="card-body p-0">
                        <div class="px-4 py-3">
                            <h4 class="text-primary">$65,563.24</h4>
                            <p class="mb-0 text-secondary"><span class="text-light-danger">38.3%-</span>
                                Last week</p>
                        </div>
                        <div class="earning-chart">
                            <div id="earningChart"></div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-sm-6 col-lg-4 col-xxl-3 ">
                <ul class=" box-list mb-4">
                    <li class="b-s-4-primary">
                        <h5>Recent Purchases</h5>
                    </li>
                    <li class="d-flex align-items-center justify-between gap-2">
                        <div class="b-1-light bg-primary-200 p-1 h-40 w-40 d-flex-center b-r-12 flex-shrink-0 overflow-hidden box-list-img">
                            <img alt="Smart Watch"
                                 class="img-fluid"
                                 src="{{asset('../assets/images/dashboard/ecommerce-dashboard/product/02.png')}}">
                        </div>
                        <div class="mg-s-45 flex-grow-1">
                            <h6 class="mb-0 text-dark-800 f-w-500 txt-ellipsis-1">Headphones</h6>
                            <p class="text-secondary-800 mb-0">AudioTech</p>
                        </div>
                        <span class="badge bg-light-primary">$120</span>
                    </li>
                    <li class="d-flex align-items-center justify-between gap-2">
                        <div class="b-1-light bg-primary-200 p-1 h-40 w-40 d-flex-center b-r-12 flex-shrink-0 overflow-hidden box-list-img">
                            <img alt="Smart Watch"
                                 class="img-fluid"
                                 src="{{asset('../assets/images/dashboard/ecommerce-dashboard/product/03.png')}}">
                        </div>
                        <div class="mg-s-45 flex-grow-1">
                            <h6 class="mb-0 text-dark-800 f-w-500 txt-ellipsis-1">Digital Camera</h6>
                            <p class="text-secondary-800 mb-0">LensPro</p>
                        </div>
                        <span class="badge bg-light-primary">$350</span>
                    </li>
                    <li class="d-flex align-items-center justify-between gap-2">
                        <div class="b-1-light bg-primary-200 p-1 h-40 w-40 d-flex-center b-r-12 flex-shrink-0 overflow-hidden box-list-img">
                            <img alt="Smart Watch"
                                 class="img-fluid"
                                 src="{{asset('../assets/images/dashboard/ecommerce-dashboard/product/01.png')}}">
                        </div>

                        <div class="mg-s-45 flex-grow-1">
                            <h6 class="mb-0 text-dark-800 f-w-500 txt-ellipsis-1">Smart Watch</h6>
                            <p class="text-secondary-800 mb-0">TechWear</p>
                        </div>
                        <span class="badge bg-light-primary">$199</span>
                    </li>
                </ul>
            </div>

            <div class="col-sm-6 col-lg-4 col-xxl-3">
                <ul class="list-box top-brand-list mb-4">
                    <li class="b-s-4-primary">
                        <h5>Top Brands</h5>
                    </li>
                    <li class="d-flex align-items-center">
                        <div class="b-1-light bg-primary-200 p-1 h-40 w-40 d-flex-center b-r-12 flex-shrink-0 overflow-hidden box-list-img">
                                <img alt="Smart Watch"
                                     class="img-fluid"
                                     src="{{asset('../assets/images/dashboard/ecommerce-dashboard/product/04.png')}}">
                        </div>
                        <div class="flex-grow-1 mg-s-45">
                            <h6 class="mb-0 f-w-500 text-dark-800 txt-ellipsis-1">Stylique</h6>
                            <p class="text-secondary-800 mb-0">Clothing</p>
                        </div>
                        <div class="text-end">
                            <span class="badge bg-light-primary">96.85%</span>
                        </div>
                    </li>

                    <li class="d-flex align-items-center">
                        <div class="b-1-light bg-primary-200 p-1 h-40 w-40 d-flex-center b-r-12 flex-shrink-0 overflow-hidden box-list-img">
                            <img alt="Smart Watch"
                                 class="img-fluid"
                                 src="{{asset('../assets/images/dashboard/ecommerce-dashboard/product/06.png')}}">
                        </div>
                        <div class="flex-grow-1 mg-s-45">
                            <h6 class="mb-0 f-w-500 text-dark-800 txt-ellipsis-1">Techspire</h6>
                            <p class="text-secondary-800 mb-0">Electronics</p>
                        </div>
                        <div class="text-end">
                            <span class="badge bg-light-success">89.42%</span>
                        </div>
                    </li>

                    <li class="d-flex align-items-center">
                        <div class="b-1-light bg-primary-200 p-1 h-40 w-40 d-flex-center b-r-12 flex-shrink-0 overflow-hidden box-list-img">
                            <img alt="Smart Watch"
                                 class="img-fluid"
                                 src="{{asset('../assets/images/dashboard/ecommerce-dashboard/product/07.png')}}">
                        </div>
                        <div class="flex-grow-1 mg-s-45">
                            <h6 class="mb-0 f-w-500 text-dark-800 txt-ellipsis-1">TrekVibe</h6>
                            <p class="text-secondary-800 mb-0">Bag's</p>
                        </div>
                        <div class="text-end">
                            <span class="badge bg-light-danger">74.30%</span>
                        </div>
                    </li>
                </ul>
            </div>

            <div class="col-sm-6 col-lg-4 col-xxl-3">
                <div class="card offer-card-box">
                    <div class="circle-ribbon circle-left ribbon-danger b-4-white">
                        50%
                    </div>

                    <div class="card-body offer-card-body overflow-hidden ">
                        <div>
                            <div class="my-3">
                                <span class="badge text-primary f-s-10 bg-white-500">Clothing</span>
                                <span class="badge text-primary f-s-10 bg-white-500">Toys</span>
                                <span class="badge text-primary f-s-10 bg-white-500">Accessories</span>
                            </div>
                            <h5 class="text-white mt-4">Super <span
                                    class="text-bg-primary  p-1 f-s-26 f-w-700 ">Kids’</span> Weekend
                                <br> <span class="text-danger highlight-word p-1">Sale</span></h5>

                        </div>
                        <div>
                            <a href="/ki-admin/product" target="_blank" class="btn btn-white  f-w-500 w-100 my-2">Shop Now</a>
                            <a class="f-s-12 f-w-500 text-white text-d-underline" href="#">Minimum purchase
                                of $30 required. Online & in-store.</a>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-sm-6 col-md-4 col-xxl-2 order-3-md">
                <div class="card service-trial-card">
                    <div class="card-body">
                        <h5 class="text-white f-w-600 txt-ellipsis-1"> AI Commerce</h5>
                        <p class="text-white mt-2 txt-ellipsis-2">Smarter Shopping, Faster Growth</p>
                        <div class=" service-img-box"></div>
                        <div class="mt-3">
                            <a href="/ki-admin/chat" target="_blank" class="btn btn-primary btn-sm w-100 text-nowrap">Start Free Trial</a>
                        </div>

                    </div>
                </div>
            </div>

            <div class="col-md-6  col-lg-4">
                <div class="card">
                    <div class="card-body pb-0">
                        <div class="mb-2">
                            <h4 class="text-primary">98.65% <span class="f-s-14 text-dark">Total sale</span></h4>
                        </div>

                        <div id="revenueChart"></div>
                    </div>
                </div>
            </div>

            <div class="col-md-8  col-xxl-6">
                <div class="card ecommerce-product-box">
                    <div class="card-body">

                        <div class="row">
                            <div class="col-sm-5">
                                <div class="row g-2 h-100">
                                    <div class="col-6">

                                        <a href="{{asset('../assets/images/dashboard/ecommerce-dashboard/product/product-01.jpg')}}" class="glightbox brand-img-box" data-glightbox="type: image; zoomable: true;">
                                            <img src="{{asset('../assets/images/dashboard/ecommerce-dashboard/product/product-01.jpg')}}" class="w-100" alt="...">
                                        </a>
                                    </div>
                                    <div class="col-6 position-relative">
                                        <div class="box-ribbon box-right">
                                            <div class="ribbonbox ribbon-danger">Latest</div>
                                        </div>
                                        <a href="{{asset('../assets/images/dashboard/ecommerce-dashboard/product/product-02.jpg')}}" class="glightbox brand-img-box" data-glightbox="type: image;">
                                            <img src="{{asset('../assets/images/dashboard/ecommerce-dashboard/product/product-02.jpg')}}" class="w-100" alt="...">
                                        </a>
                                    </div>
                                    <div class="col-3">
                                        <a href="{{asset('../assets/images/dashboard/ecommerce-dashboard/product/product-03.jpg')}}" class="glightbox brand-img-box" data-glightbox="type: image;">
                                            <img src="{{asset('../assets/images/dashboard/ecommerce-dashboard/product/product-03.jpg')}}" class="w-100" alt="...">
                                        </a>
                                    </div>
                                    <div class="col-3">
                                        <a href="{{asset('../assets/images/dashboard/ecommerce-dashboard/product/product-04.jpg')}}" class="glightbox brand-img-box" data-glightbox="type: image;">
                                            <img src="{{asset('../assets/images/dashboard/ecommerce-dashboard/product/product-04.jpg')}}" class="w-100" alt="...">
                                        </a>
                                    </div>
                                    <div class="col-3">
                                        <a href="{{asset('../assets/images/dashboard/ecommerce-dashboard/product/product-05.jpg')}}" class="glightbox brand-img-box" data-glightbox="type: image;">
                                            <img src="{{asset('../assets/images/dashboard/ecommerce-dashboard/product/product-05.jpg')}}" class="w-100" alt="...">
                                        </a>
                                    </div>
                                    <div class="col-3">
                                        <a href="{{asset('../assets/images/dashboard/ecommerce-dashboard/product/product-06.jpg')}}" class="glightbox brand-img-box" data-glightbox="type: image;">
                                            <img src="{{asset('../assets/images/dashboard/ecommerce-dashboard/product/product-06.jpg')}}" class="w-100 rounded" alt="...">
                                        </a>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-7">
                                <div class="my-3">
                                    <h3 class="text-dark-800 f-w-700 txt-ellipsis-1">$58,902.90 <span
                                            class="f-s-16 f-w-500 text-secondary">Last week</span></h3>
                                    <div class="custom-progress-container">
                                        <div class="progress-bar"></div>
                                        <div class="progress-bar"></div>
                                        <div class="progress-bar"></div>
                                        <div class="progress-bar"></div>
                                    </div>
                                </div>

                                <h6>Choose tags</h6>
                                <div class="form-selectgroup products-tags-list my-3">
                                    <label class="select-items">
                                        <input checked class="select-input" type="checkbox">
                                        <span class="select-box">
                                                        <span class="selectitem">○  New Arrivals</span>
                                                    </span>
                                    </label>
                                    <label class="select-items">
                                        <input class="select-input" type="checkbox">
                                        <span class="select-box">
                                                        <span class="selectitem">○  Best Sellers</span>
                                                    </span>
                                    </label>
                                    <label class="select-items">
                                        <input checked class="select-input" type="checkbox">
                                        <span class="select-box">
                                                        <span class="selectitem">○  Trending</span>
                                                    </span>
                                    </label>
                                    <label class="select-items">
                                        <input class="select-input" type="checkbox">
                                        <span class="select-box">
                                                        <span class="selectitem">○  Exclusive</span>
                                                    </span>
                                    </label>
                                    <label class="select-items">
                                        <input checked class="select-input" type="checkbox">
                                        <span class="select-box">
                                                        <span class="selectitem">○  On Sale</span>
                                                    </span>
                                    </label>
                                    <label class="select-items">
                                        <input class="select-input" type="checkbox">
                                        <span class="select-box">
                                                        <span class="selectitem">○   Free Shipping</span>
                                                    </span>
                                    </label>
                                </div>


                            </div>
                        </div>


                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- modal -->


@endsection

@section('script')

    <!-- apexcharts js-->
    <script src="{{asset('assets/vendor/apexcharts/apexcharts.min.js')}}"></script>

    <!-- Glight js -->
    <script src="{{asset('assets//vendor/glightbox/glightbox.min.js')}}"></script>

    <!-- Ecommerce js-->
    <script src="{{asset('assets/js/ecommerce_dashboard.js')}}"></script>

@endsection
