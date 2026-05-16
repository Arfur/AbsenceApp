<!DOCTYPE html>
<html lang="en">

<head>
    <!-- All meta and title start-->
@include('layout.head')
<!-- meta and title end-->

<!-- css start !-->
@include('layout.css')
<!-- css end !-->
</head>

<body>
<!-- Loader start-->
<div class="app-wrapper">
    <!-- Loader start-->
    <div class="loader-wrapper">
        <div class="loader_24"></div>
    </div>
    <!-- Loader end-->

    <!-- Menu Navigation start -->
@include('z_ki-admin.layout.sidebar')
<!-- Menu Navigation end -->


    <div class="app-content">
        <!-- Header Section start -->
    @include('z_ki-admin.layout.header')
    <!-- Header Section end -->

        <!-- Main Section start -->
        <main>
            {{-- main body content --}}
            @yield('main-content')
        </main>
        <!-- Main Section end -->
    </div>

    <!-- tap on top -->
    <div class="go-top">
      <span class="progress-value">
        <i class="ti ti-arrow-up"></i>
      </span>
    </div>

    <!-- Footer Section start -->
     @include('z_ki-admin.layout.footer')
    <!-- Footer Section end -->
</div>

<!--customizer-->
<div id="customizer"></div>

<!-- scripts start-->
@include('layout.script')
<!-- scripts end-->
</body>



</html>
