<template>
    <div id="app">
        <form>
            <input type="text" />
            <button>Add Product</button>
        </form>
    </div>
</template>

<script>
    import Home from './components/Home.vue';

    export default {
      
    };
</script>

<style>
    input{
        
    }
    button{
        display: inline-block;
        border: none;
        padding: 7px 20px;
        cursor: pointer;
        color: #ffffff;
        background-color: #555; 
    }

    button:hover{
        background-color: #666;
    }
</style>

