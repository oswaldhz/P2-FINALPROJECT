import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5178/api',
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const login = (email, password) =>
  api.post('/auth/login', { email, password }).then((r) => r.data);

export const register = (email, nombre, password) =>
  api.post('/auth/register', { email, nombre, password }).then((r) => r.data);

export const fetchReservas = () => api.get('/reservas').then((r) => r.data);

export const fetchEquipos = () => api.get('/equipos').then((r) => r.data);

export const createReserva = (payload) => api.post('/reservas', payload).then((r) => r.data);

export default api;
