--
-- PostgreSQL database dump
--

\restrict w0qMYRhJdX3t4qFiJq9jmP9ljhZbdO7S6YLKKiRrBwXMVdafhHdTaOTS91vjLuz

-- Dumped from database version 18.6
-- Dumped by pg_dump version 18.6

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: attendance; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.attendance (
    attendance_id integer NOT NULL,
    session_id integer NOT NULL,
    client_id integer NOT NULL,
    attended boolean NOT NULL,
    rating integer,
    CONSTRAINT attendance_rating_check CHECK (((rating IS NULL) OR ((rating >= 1) AND (rating <= 5))))
);


ALTER TABLE public.attendance OWNER TO postgres;

--
-- Name: attendance_attendance_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.attendance_attendance_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.attendance_attendance_id_seq OWNER TO postgres;

--
-- Name: attendance_attendance_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.attendance_attendance_id_seq OWNED BY public.attendance.attendance_id;


--
-- Name: clients; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.clients (
    client_id integer NOT NULL,
    first_name character varying(50) NOT NULL,
    last_name character varying(50) NOT NULL,
    birth_date date NOT NULL,
    phone character varying(20),
    email character varying(100),
    registration_date date NOT NULL,
    status character varying(20) NOT NULL,
    CONSTRAINT clients_status_check CHECK (((status)::text = ANY (ARRAY[('active'::character varying)::text, ('inactive'::character varying)::text])))
);


ALTER TABLE public.clients OWNER TO postgres;

--
-- Name: clients_client_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.clients_client_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.clients_client_id_seq OWNER TO postgres;

--
-- Name: clients_client_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.clients_client_id_seq OWNED BY public.clients.client_id;


--
-- Name: membership_types; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.membership_types (
    membership_type_id integer NOT NULL,
    name character varying(100) NOT NULL,
    duration_days integer NOT NULL,
    price numeric(10,2) NOT NULL,
    visits_limit integer,
    CONSTRAINT membership_types_duration_days_check CHECK ((duration_days > 0)),
    CONSTRAINT membership_types_price_check CHECK ((price >= (0)::numeric)),
    CONSTRAINT membership_types_visits_limit_check CHECK (((visits_limit IS NULL) OR (visits_limit > 0)))
);


ALTER TABLE public.membership_types OWNER TO postgres;

--
-- Name: membership_types_membership_type_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.membership_types_membership_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.membership_types_membership_type_id_seq OWNER TO postgres;

--
-- Name: membership_types_membership_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.membership_types_membership_type_id_seq OWNED BY public.membership_types.membership_type_id;


--
-- Name: memberships; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.memberships (
    membership_id integer NOT NULL,
    client_id integer NOT NULL,
    membership_type_id integer NOT NULL,
    start_date date NOT NULL,
    end_date date NOT NULL,
    status character varying(20) NOT NULL,
    CONSTRAINT memberships_check CHECK ((end_date >= start_date)),
    CONSTRAINT memberships_status_check CHECK (((status)::text = ANY (ARRAY[('active'::character varying)::text, ('frozen'::character varying)::text, ('expired'::character varying)::text])))
);


ALTER TABLE public.memberships OWNER TO postgres;

--
-- Name: memberships_membership_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.memberships_membership_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.memberships_membership_id_seq OWNER TO postgres;

--
-- Name: memberships_membership_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.memberships_membership_id_seq OWNED BY public.memberships.membership_id;


--
-- Name: payments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.payments (
    payment_id integer NOT NULL,
    client_id integer NOT NULL,
    membership_id integer,
    payment_date date NOT NULL,
    amount numeric(10,2) NOT NULL,
    payment_method character varying(20) NOT NULL,
    status character varying(20) NOT NULL,
    CONSTRAINT payments_amount_check CHECK ((amount >= (0)::numeric)),
    CONSTRAINT payments_payment_method_check CHECK (((payment_method)::text = ANY (ARRAY[('card'::character varying)::text, ('cash'::character varying)::text, ('transfer'::character varying)::text]))),
    CONSTRAINT payments_status_check CHECK (((status)::text = ANY (ARRAY[('paid'::character varying)::text, ('pending'::character varying)::text, ('refunded'::character varying)::text])))
);


ALTER TABLE public.payments OWNER TO postgres;

--
-- Name: payments_payment_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.payments_payment_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.payments_payment_id_seq OWNER TO postgres;

--
-- Name: payments_payment_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.payments_payment_id_seq OWNED BY public.payments.payment_id;


--
-- Name: trainers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.trainers (
    trainer_id integer NOT NULL,
    first_name character varying(50) NOT NULL,
    last_name character varying(50) NOT NULL,
    specialization character varying(100) NOT NULL,
    hire_date date NOT NULL,
    salary numeric(10,2) NOT NULL,
    CONSTRAINT trainers_salary_check CHECK ((salary > (0)::numeric))
);


ALTER TABLE public.trainers OWNER TO postgres;

--
-- Name: trainers_trainer_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.trainers_trainer_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.trainers_trainer_id_seq OWNER TO postgres;

--
-- Name: trainers_trainer_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.trainers_trainer_id_seq OWNED BY public.trainers.trainer_id;


--
-- Name: workout_sessions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.workout_sessions (
    session_id integer NOT NULL,
    workout_id integer NOT NULL,
    trainer_id integer NOT NULL,
    session_date date NOT NULL,
    start_time time without time zone NOT NULL,
    room character varying(30) NOT NULL,
    capacity integer NOT NULL,
    CONSTRAINT workout_sessions_capacity_check CHECK ((capacity > 0))
);


ALTER TABLE public.workout_sessions OWNER TO postgres;

--
-- Name: workout_sessions_session_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.workout_sessions_session_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.workout_sessions_session_id_seq OWNER TO postgres;

--
-- Name: workout_sessions_session_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.workout_sessions_session_id_seq OWNED BY public.workout_sessions.session_id;


--
-- Name: workouts; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.workouts (
    workout_id integer NOT NULL,
    name character varying(100) NOT NULL,
    category character varying(50) NOT NULL,
    difficulty character varying(20) NOT NULL,
    duration_minutes integer NOT NULL,
    max_clients integer NOT NULL,
    CONSTRAINT workouts_difficulty_check CHECK (((difficulty)::text = ANY (ARRAY[('beginner'::character varying)::text, ('medium'::character varying)::text, ('advanced'::character varying)::text]))),
    CONSTRAINT workouts_duration_minutes_check CHECK ((duration_minutes > 0)),
    CONSTRAINT workouts_max_clients_check CHECK ((max_clients > 0))
);


ALTER TABLE public.workouts OWNER TO postgres;

--
-- Name: workouts_workout_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.workouts_workout_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.workouts_workout_id_seq OWNER TO postgres;

--
-- Name: workouts_workout_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.workouts_workout_id_seq OWNED BY public.workouts.workout_id;


--
-- Name: attendance attendance_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.attendance ALTER COLUMN attendance_id SET DEFAULT nextval('public.attendance_attendance_id_seq'::regclass);


--
-- Name: clients client_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clients ALTER COLUMN client_id SET DEFAULT nextval('public.clients_client_id_seq'::regclass);


--
-- Name: membership_types membership_type_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.membership_types ALTER COLUMN membership_type_id SET DEFAULT nextval('public.membership_types_membership_type_id_seq'::regclass);


--
-- Name: memberships membership_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.memberships ALTER COLUMN membership_id SET DEFAULT nextval('public.memberships_membership_id_seq'::regclass);


--
-- Name: payments payment_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.payments ALTER COLUMN payment_id SET DEFAULT nextval('public.payments_payment_id_seq'::regclass);


--
-- Name: trainers trainer_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.trainers ALTER COLUMN trainer_id SET DEFAULT nextval('public.trainers_trainer_id_seq'::regclass);


--
-- Name: workout_sessions session_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.workout_sessions ALTER COLUMN session_id SET DEFAULT nextval('public.workout_sessions_session_id_seq'::regclass);


--
-- Name: workouts workout_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.workouts ALTER COLUMN workout_id SET DEFAULT nextval('public.workouts_workout_id_seq'::regclass);


--
-- Name: attendance attendance_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.attendance
    ADD CONSTRAINT attendance_pkey PRIMARY KEY (attendance_id);


--
-- Name: attendance attendance_session_id_client_id_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.attendance
    ADD CONSTRAINT attendance_session_id_client_id_key UNIQUE (session_id, client_id);


--
-- Name: clients clients_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clients
    ADD CONSTRAINT clients_email_key UNIQUE (email);


--
-- Name: clients clients_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clients
    ADD CONSTRAINT clients_pkey PRIMARY KEY (client_id);


--
-- Name: membership_types membership_types_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.membership_types
    ADD CONSTRAINT membership_types_pkey PRIMARY KEY (membership_type_id);


--
-- Name: memberships memberships_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.memberships
    ADD CONSTRAINT memberships_pkey PRIMARY KEY (membership_id);


--
-- Name: payments payments_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.payments
    ADD CONSTRAINT payments_pkey PRIMARY KEY (payment_id);


--
-- Name: trainers trainers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.trainers
    ADD CONSTRAINT trainers_pkey PRIMARY KEY (trainer_id);


--
-- Name: workout_sessions workout_sessions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.workout_sessions
    ADD CONSTRAINT workout_sessions_pkey PRIMARY KEY (session_id);


--
-- Name: workouts workouts_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.workouts
    ADD CONSTRAINT workouts_pkey PRIMARY KEY (workout_id);


--
-- Name: idx_attendance_client_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_attendance_client_id ON public.attendance USING btree (client_id);


--
-- Name: idx_attendance_session_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_attendance_session_id ON public.attendance USING btree (session_id);


--
-- Name: idx_memberships_client_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_memberships_client_id ON public.memberships USING btree (client_id);


--
-- Name: idx_memberships_type_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_memberships_type_id ON public.memberships USING btree (membership_type_id);


--
-- Name: idx_payments_client_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_payments_client_id ON public.payments USING btree (client_id);


--
-- Name: idx_payments_date; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_payments_date ON public.payments USING btree (payment_date);


--
-- Name: idx_sessions_trainer_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_sessions_trainer_id ON public.workout_sessions USING btree (trainer_id);


--
-- Name: idx_sessions_workout_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_sessions_workout_id ON public.workout_sessions USING btree (workout_id);


--
-- Name: attendance attendance_client_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.attendance
    ADD CONSTRAINT attendance_client_id_fkey FOREIGN KEY (client_id) REFERENCES public.clients(client_id) ON DELETE CASCADE;


--
-- Name: attendance attendance_session_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.attendance
    ADD CONSTRAINT attendance_session_id_fkey FOREIGN KEY (session_id) REFERENCES public.workout_sessions(session_id) ON DELETE CASCADE;


--
-- Name: memberships memberships_client_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.memberships
    ADD CONSTRAINT memberships_client_id_fkey FOREIGN KEY (client_id) REFERENCES public.clients(client_id);


--
-- Name: memberships memberships_membership_type_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.memberships
    ADD CONSTRAINT memberships_membership_type_id_fkey FOREIGN KEY (membership_type_id) REFERENCES public.membership_types(membership_type_id);


--
-- Name: payments payments_client_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.payments
    ADD CONSTRAINT payments_client_id_fkey FOREIGN KEY (client_id) REFERENCES public.clients(client_id);


--
-- Name: payments payments_membership_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.payments
    ADD CONSTRAINT payments_membership_id_fkey FOREIGN KEY (membership_id) REFERENCES public.memberships(membership_id);


--
-- Name: workout_sessions workout_sessions_trainer_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.workout_sessions
    ADD CONSTRAINT workout_sessions_trainer_id_fkey FOREIGN KEY (trainer_id) REFERENCES public.trainers(trainer_id);


--
-- Name: workout_sessions workout_sessions_workout_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.workout_sessions
    ADD CONSTRAINT workout_sessions_workout_id_fkey FOREIGN KEY (workout_id) REFERENCES public.workouts(workout_id);


--
-- PostgreSQL database dump complete
--

\unrestrict w0qMYRhJdX3t4qFiJq9jmP9ljhZbdO7S6YLKKiRrBwXMVdafhHdTaOTS91vjLuz

