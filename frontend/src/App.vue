<script setup>
import DataTable from "@/volt/DataTable.vue";
import Column from "primevue/column";
import ToggleSwitch from "@/volt/ToggleSwitch.vue";
import ConfirmDialog from "./volt/ConfirmDialog.vue";
import { useConfirm } from "primevue/useconfirm";
import { useToast } from "primevue/usetoast";
import { ref, onMounted, computed } from "vue";
import Button from "./volt/Button.vue";
import axios from "axios";

const weatherData = ref([]);

const selectedRecords = ref([]);
const API_URL = "https://localhost:7235/api/weather/all";
const DELETE_URL = "https://localhost:7235/api/weather/delete-multiple";

const metaKey = ref(true);
const confirm = useConfirm();
const toast = useToast();

const isDisabled = computed(() => {
  return selectedRecords.value?.length === 0;
});

const formatDate = (value) => {
  const date = new Date(value);
  return date.toLocaleString("en-US", {
    month: "long", // Full month name (e.g., March)
    day: "numeric", // Day of the month (e.g., 31)
    year: "numeric", // Full year (e.g., 2025)
    hour: "numeric", // Hour in 12-hour format (e.g., 5)
    minute: "2-digit", // Two-digit minute (e.g., 05)
    hour12: true, // 12-hour format (AM/PM)
  });
};

const fetchWeatherData = async () => {
  try {
    const response = await axios.get(API_URL);
    weatherData.value = response.data;
    console.log(weatherData);
  } catch (error) {
    console.error("Error fetching weather data:", error);
  }
};

const deleteSelected = async () => {
  const idsToDelete = selectedRecords.value.map((p) => p.id);

  try {
    const response = await axios.post(DELETE_URL, idsToDelete);
    if (response.status === 204) {
      console.log("Deleted successfully");
      await fetchWeatherData();
      // Optionally refetch or update your local `products` list here
    } else {
      console.warn("Unexpected response:", response);
    }
  } catch (error) {
    console.error("Deletion error:", error);
  }
};

const confirmDelete = () => {
  confirm.require({
    message: "Are you sure you want to delete?",
    header: "Confirmation",
    rejectProps: {
      label: "Cancel",
      severity: "secondary",
      outlined: true,
    },
    acceptProps: {
      label: "Delete",
    },
    accept: () => {
      deleteSelected();
      toast.add({
        severity: "info",
        summary: "Deleted",
        detail: "Successfully Deleted",
        life: 3000,
      });
    },
    reject: () => {
      toast.add({
        severity: "error",
        summary: "Cancelled",
        detail: "You have cancelled",
        life: 3000,
      });
    },
  });
};

onMounted(fetchWeatherData);
</script>

<template>
  <main>
    <div class="w-6xl rounded-lg border border-gray-500 shadow-xl p-4">
      <div class="flex justify-center items-center mb-6 gap-2">
        <ToggleSwitch v-model="metaKey" inputId="input-metakey" />
        <label for="input-metakey" class="text-black dark:text-black"
          >MetaKey</label
        >
      </div>
      <ConfirmDialog />
      <DataTable
        v-model:selection="selectedRecords"
        :value="weatherData"
        selectionMode="multiple"
        :metaKeySelection="metaKey"
        dataKey="id"
        scrollable
        scrollHeight="400px"
        :virtualScrollerOptions="{ itemSize: 44 }"
        tableStyle="min-width: 50rem"
      >
        <Column field="id" header="ID" style="width: 5%; height: 44px"></Column>
        <Column field="date" header="Date" style="width: 23.75%; height: 44px"
          ><template #body="slotProps">
            {{ formatDate(slotProps.data.date) }}
          </template></Column
        >
        <Column
          field="temperature"
          header="Temperature"
          style="width: 23.75%; height: 44px"
        >
        </Column>
        <Column
          field="humidity"
          header="Humidity"
          style="width: 23.75%; height: 44px"
        ></Column>
        <Column
          field="windSpeed"
          header="Wind Speed"
          style="width: 23.75%; height: 44px"
        ></Column>
      </DataTable>
      <div class="flex row pt-6">
        <Button
          label="Delete"
          :disabled="isDisabled"
          @click="confirmDelete"
        ></Button>
      </div>
    </div>
  </main>
</template>

<style scoped></style>
